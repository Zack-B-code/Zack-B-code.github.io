/* ************************************************************************* *
 *                                                                           *
 *  zb_proxy.cpp:                                                            *
 *    This is a program for making a multi threaded			                 *
 *	  appilaction-layer proxy firewall.										 *
 *                                                                           *
 *                                                                           *
 *  Compile:                                                                 *
 *    In Project->Setting->C/C++->CodeGenartion(in Category)                 *
 *            ->Select Multi-threaded for runtime library                    *
 *                                                                           *
 *  Coded by: Zack Bahn														 *
 *																			 *
 *                                                                           *
 * ************************************************************************* */

 //----- Include files ---------------------------------------------------------
#include <winsock2.h>
#include <stdio.h>        
#include <stdlib.h>        
#include <string.h>  
#include <string> 
#include <fcntl.h>          
#include <sys/stat.h> 
#include <time.h> 
#include <stddef.h> 
#include <process.h> 
#include <windows.h> 
#include <io.h> 
#include <iostream>
#include <sys\timeb.h>
#include <ws2tcpip.h>

#pragma comment(lib, "Ws2_32.lib")

 //----- Defines -------------------------------------------------------------//
#define PORT_NUM       9080
#define PROXY_PORT     10080
#define SERVER_PORT    5080
#define BUF_SIZE       1200
#define SES_SIZE       100
//#define IP_ADDRESS   "146.163.150.113"
#define IP_ADDRESS      "192.168.1.126"

int server_socket;          // server socket descriptor
int client_socket;          // client socket descriptor
int client_s1[SES_SIZE];    // client socket array to keep track of session
int client_s2[SES_SIZE];    // client socket array to keep track of session
unsigned int ses_id;        // session id

// hazardous contents string declared globally here to use in functions later
char hazardous_contents_CS_01[256] = "admin_login.html;";
char hazardous_contents_CS_02[256] = "password.txt;";
char hazardous_contents_SC_01[256] = "admin_login.html;";
char hazardous_contents_SC_02[256] = "password.txt;";

//----- Function prototypes -------------------------------------------------
/* FOR WIN --------------------------------------------------------------- */
void SendData(void* dummy);
void RecieveData(void* dummy);
int FindSession(void);
bool hasHazardousContent(const char* data, int dataSize, const char* hazardousContent);


int main(void)
{
    // hazardous contents string declarations
    char hazardous_contents_CS_01[256] = "admin_login.html;";
    char hazardous_contents_CS_02[256] = "password.txt;";
    char hazardous_contents_SC_01[256] = "admin_login.html;";
    char hazardous_contents_SC_02[256] = "password.txt;";

    int status;

    struct sockaddr_in  server_addr1;
    struct sockaddr_in  client_addr1;
    struct sockaddr_in  server_addr2;
    struct sockaddr_in  client_addr2;
    int addr_len1;                      // Internet address length
    int addr_len2;                      // Internet address length
    char temp_buf[BUF_SIZE];

    int n_session;
    int n_error = FALSE;
    unsigned int ses_id = 0;
    unsigned int proxy_port;

    SYSTEMTIME  stSystemTime;
    int         nHour;
    IN_ADDR     client_address;         // client source address

    // for (int i = 0; i < 256; i++)
    // {
    //     hazardous_contents_CS_01[i] = '\0';
    //     hazardous_contents_CS_02[i] = '\0';
    //     hazardous_contents_SC_01[i] = '\0';
    //     hazardous_contents_SC_02[i] = '\0';
    // }

    for (n_session = 0; n_session < SES_SIZE; n_session++)
    {
        client_s1[n_session] = -1;
        client_s2[n_session] = -1;
    }


    printf("Proxy server connection being created now!\n");

    WORD wVersionRequested = MAKEWORD(1, 1);
    WSADATA wsaData;
    if (WSAStartup(wVersionRequested, &wsaData) != 0) {
        printf("WSAStartup failed\n");
        return -1;
    }


    server_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket < 0)
    {
        printf("Error at socket(): %ld\n", WSAGetLastError());
        WSACleanup();
        n_error = TRUE;
    }

    server_addr1.sin_family = AF_INET;
    server_addr1.sin_port = htons(SERVER_PORT);
    server_addr1.sin_addr.s_addr = htonl(INADDR_ANY);
    status = bind(server_socket, (struct sockaddr*)&server_addr1, sizeof(server_addr1));
    if (status > 0)
    {
        printf("There's an error in the 1st BIND function!\n");
        n_error = TRUE;
    }

    // Listen for connections and then accept later
    status = listen(server_socket, 100);
    if (status < 0)
    {
        printf("There's an error in the 1st LISTEN function!\n");
        n_error = TRUE;
    }


    while (n_error == FALSE)
    {
        // if program makes it here, connection is successful
        printf("Web proxy is ready! \n");
        printf("The port for the proxy connection is: %d\n", PORT_NUM);

        // Wait for incomming SMTP request from client 
        addr_len1 = sizeof(client_addr1);
        client_socket = accept(server_socket, (struct sockaddr*)&client_addr1, &addr_len1);

        // Check source address of connecting client
        memcpy(&client_address, &client_addr1.sin_addr.s_addr, 4);
        printf("The IP address of the connecting client is: %s\n", inet_ntoa(client_address));

        // Find available session slot
        n_session = FindSession();

        if (n_session >= 0)
        {
            printf("An available session slot was found: %d\n\n", n_session);

            client_s1[n_session] = client_socket;
            // Create a new socket for client_s2
            client_s2[n_session] = socket(AF_INET, SOCK_STREAM, 0); 

            if (client_s1[n_session] <= 0 || client_s2[n_session] <= 0)
            {
                printf("There's an error in the 1st ACCEPT or SOCKET function!\n");
                n_error = TRUE;
            }
            else
            {
                // Convert UTC to CST in US 
                GetSystemTime(&stSystemTime);
                if (stSystemTime.wHour >= 6)
                {
                    nHour = stSystemTime.wHour - 6;
                }
                else
                {
                    nHour = stSystemTime.wHour + 18;
                }

                // Check again to ensure that session is still available
                if (client_s2[n_session] < 0)
                {
                    printf("There's an error in the 2nd SOCKET function!\n");
                    closesocket(client_s1[n_session]);
                    n_error = TRUE;
                }
                else
                {
                    // Assign proxy socket to address information, and then bind it
                    proxy_port = (PROXY_PORT + ses_id - 1);
                    client_addr2.sin_family = AF_INET;
                    client_addr2.sin_port = htons(proxy_port);
                    client_addr2.sin_addr.s_addr = htonl(INADDR_ANY);

                    // Add this line to allow reuse of the address
                    int reuse_addr = 1;
                    setsockopt(client_s2[n_session], SOL_SOCKET, SO_REUSEADDR, (const char*)&reuse_addr, sizeof(reuse_addr));

                    status = bind(client_s2[n_session], (struct sockaddr*)&client_addr2, sizeof(client_addr2));
                    if (status < 0)
                    {
                        printf("There's an error in the 2nd BIND function detected!\n");
                        n_error = TRUE;
                    }

                    server_addr2.sin_family = AF_INET;
                    server_addr2.sin_addr.s_addr = inet_addr(IP_ADDRESS);
                    server_addr2.sin_port = htons(SERVER_PORT);
                    addr_len2 = sizeof(server_addr2);

                    status = connect(client_s2[n_session], (struct sockaddr*)&server_addr2, sizeof(server_addr2));
                    if (status < 0)
                    {
                        printf("Error when connecting to the server! Error code: %d\n", WSAGetLastError());             

                        n_error = TRUE;
                        closesocket(client_s1[n_session]);
                        closesocket(client_s2[n_session]);
                    }
                    else
                    {
                        _beginthread(SendData, 1000, (void*)n_session);
                        _beginthread(RecieveData, 1000, (void*)n_session);
                    }

                }
            }
        }
        else
        {
            strcpy(temp_buf, "All of the channels busy!");
            send(client_socket, temp_buf, strlen(temp_buf), 0);
            closesocket(client_socket);
            client_socket = -1;
        }
    }

    // Cleanup
    status = closesocket(server_socket);
    WSACleanup();

    return 0;
}


void SendData(void* slot)
{
    int status1;
    int status2;

    int n_error2 = FALSE;
    char out_buf[BUF_SIZE];
    SYSTEMTIME stSystemTime;
    int nHour;

    // Get the current session ID
    unsigned int my_ses_id = ses_id;

    while (n_error2 == FALSE)
    {
        status1 = recv(client_s1[(int)slot], out_buf, BUF_SIZE, 0);
        if (status1 > 0) 
        {
            printf("Received %d bytes from client_s1[%d]\n", status1, (int)slot);
            status2 = send(client_s2[(int)slot], out_buf, status1, 0);
            printf("Sent %d bytes to client_s2[%d]\n", status2, (int)slot);

            if (hasHazardousContent(out_buf, status1, hazardous_contents_SC_01) || hasHazardousContent(out_buf, status2, hazardous_contents_SC_02)) 
            {
                // Respond with HTTP 401: Unauthorized Access
                const char* response = "HTTP/1.1 401 Unauthorized\r\n\r\nUnauthorized Access\r\n";
                send(client_s1[(int)slot], response, strlen(response), 0);
                closesocket(client_s1[(int)slot]);
                return;
            }

            if ((status1 <= 0) || (status2 <= 0))
            {
                printf("Error in send/receive operation: status1=%d, status2=%d, errno=%d\n", status1, status2, WSAGetLastError());
                n_error2 = TRUE;
            }
        }
        else if (status1 == 0) 
        {
            printf("Connection closed by client_s1[%d]\n", (int)slot);
            n_error2 = TRUE;
        }
    }

    // Get the current system time
    GetSystemTime(&stSystemTime);
    // Convert UTC to CST in US 
    if (stSystemTime.wHour >= 6)
    {
        nHour = stSystemTime.wHour - 6;
    }
    else
    {
        nHour = stSystemTime.wHour + 18;
    }
    printf("This Incoming socket is closed (session ID: %d) at %d:%d:%d (UTC)\n", my_ses_id, nHour, stSystemTime.wMinute, stSystemTime.wSecond);

    // Release socket slot for next request 
    closesocket(client_s1[(int)slot]);
    client_s1[(int)slot] = -1;

    // End this thread
    _endthread();
}

void RecieveData(void* slot)
{
    int status1;
    int status2;

    int n_error2 = FALSE;
    char in_buf[BUF_SIZE];
    SYSTEMTIME stSystemTime;
    int nHour;

    // Get the current session ID
    unsigned int my_ses_id = ses_id;

    while (n_error2 == FALSE)
    {
        status2 = recv(client_s2[(int)slot], in_buf, BUF_SIZE, 0);
        if (status2 > 0) 
        {
            printf("Received %d bytes from client_s2[%d]\n", status2, (int)slot);
            status1 = send(client_s1[(int)slot], in_buf, status2, 0);
            printf("Sent %d bytes to client_s1[%d]\n", status1, (int)slot);

            // Check for hazardous content
            if (hasHazardousContent(in_buf, status2, hazardous_contents_CS_01) || hasHazardousContent(in_buf, status2, hazardous_contents_CS_02)) 
            {
                // Respond with HTTP 401: Unauthorized Access
                const char* response = "HTTP/1.1 401 Unauthorized\r\n\r\nUnauthorized Access\r\n";
                send(client_s2[(int)slot], response, strlen(response), 0);
                closesocket(client_s2[(int)slot]);
                return;
            }

            if ((status1 <= 0) || (status2 <= 0))
            {
                printf("Error in send/receive operation: status1=%d, status2=%d, errno=%d\n", status1, status2, WSAGetLastError());
                n_error2 = TRUE;
            }
        }
        else if (status2 == 0) 
        {
            printf("Connection closed by client_s2[%d]\n", (int)slot);
            n_error2 = TRUE;
        }
    }

    

    // Get the current system time
    GetSystemTime(&stSystemTime);
    // Convert UTC to CST in US 
    if (stSystemTime.wHour >= 6)
    {
        nHour = stSystemTime.wHour - 6;
    }
    else
    {
        nHour = stSystemTime.wHour + 18;
    }
    printf("This Incoming socket is closed (session ID: %d) at %d:%d:%d (UTC)\n", my_ses_id, nHour, stSystemTime.wMinute, stSystemTime.wSecond);

    // Release socket slot for next request 
    closesocket(client_s2[(int)slot]);
    client_s2[(int)slot] = -1;

    // End this thread
    _endthread();
}

int FindSession(void)
{

    int i;
    int n_available_slot_num = -1;

    for (i = 0; i < SES_SIZE; i++)
    {
        if (client_s1[i] < 0)
        {
            n_available_slot_num = i;
            break;
            
        }
    }

    return (n_available_slot_num);
}

bool hasHazardousContent(const char* data, int dataSize, const char* hazardousContent) {
    // string-based hazardous content check
    return (strstr(data, hazardousContent) != nullptr);
}