// by Zack Bahn
#include<iostream>
#include<iomanip>
#include<string>
#include<cmath>
#include<random>
using namespace std;


void Printpath(char arr[60][60]);
int Randomness(int windL);
void Determinedirection(int dir, char arr[60][60], int& stp, int dist, int& p, int& q);

int main()
{                                                                                                                                                                                //A                          
	char array[60][60];
	for (int x = 0; x < 60; x++)
	{
		for (int y = 0; y < 60; y++)
		{
			array[x][y] = '.';
		}
	}
	array[30][30] = 'A';

	int distance = 1;
	int direction = 1;
	int windyness = 1;
	bool cont = true;

	while (cont)
	{
		cout << "Input a distance anywhere from 1 to 60: ";
		cin >> distance;
		while (distance < 1 || distance > 60)
		{
			cout << "Try again. Enter a number from 1 and 60: ";
			cin >> distance;
		}

		cout << "  1  2  3" << endl << "   \\ | / " << endl << "  8--+--4" << endl << "   / | \\ " << endl << "  7  6  5" << endl;
		cout << "Enter a direction from 1 to 8: ";
		cin >> direction;
		while (direction < 1 || direction > 8)
		{
			cout << "Try again. Enter a direction from 1 and 8: ";
			cin >> direction;
		}

		cout << "How straight do you want the path to be? (1 = mostly straight, 2 = somewhat straight, 3 = not straight at all): ";
		cin >> windyness;
		while (windyness < 1 || windyness > 3)
		{
			cout << "Try again. Enter a value from 1 and 3: ";
			cin >> windyness;
		}

		int nxtsp = 1;

		char array[60][60];
		for (int x = 0; x < 60; x++)
		{
			for (int y = 0; y < 60; y++)
			{
				array[x][y] = '.';
			}
		}
		array[30][30] = 'A';

		int p = 30;
		int q = 30;
		Printpath(array);
		for (int stop = 0; stop < distance; stop++) 
		{
			nxtsp = Randomness(windyness);
			if (nxtsp == 1) // in direction given by user
			{
				Determinedirection(direction, array, stop, distance, p, q);
			}
			if (nxtsp == 2) // in same direction as the previous spot 
			{
				Determinedirection(direction, array, stop, distance, p, q);
			}
			if (nxtsp == 3) // turn one direction left
			{
				if (direction == 1)
				{
					direction = direction + 7;
					Determinedirection(direction, array, stop, distance, p, q);
				}
				else
				{
					direction = direction - 1;
					Determinedirection(direction, array, stop, distance, p, q);
				}
			}
			if (nxtsp == 4) // turn one direction right
			{
				if (direction == 8)
				{
					direction = direction - 7;
					Determinedirection(direction, array, stop, distance, p, q);
				}
				else
				{
					direction = direction + 1;
					Determinedirection(direction, array, stop, distance, p, q);
				}
			}
			
			if (stop < distance) 
			{
				system("cls");
			}
			Printpath(array);
		}


	}

	return 0;
}

int Randomness(int windL)
{
	int nextspace = 1;
	int weight = 1;

	if (windL == 1)
	{
		weight = rand() % 20;
		switch (weight)
		{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				nextspace = 1;
				break;
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
				nextspace = 2;
				break;
			case 18:
				nextspace = 3;
				break;
			case 19:
				nextspace = 4;
				break;
		}

	}
	if (windL == 2)
	{
		weight = rand() % 10;
		switch (weight)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			nextspace = 1;
			break;
		case 4:
		case 5:
			nextspace = 2;
			break;
		case 6:
		case 7:
			nextspace = 3;
			break;
		case 8:
		case 9:
			nextspace = 4;
			break;
		}
	}
	if (windL == 3)
	{
		weight = rand() % 10;
		switch (weight)
		{
		case 0:
		case 1:
			nextspace = 1;
			break;
		case 2:
		case 3:
			nextspace = 2;
			break;
		case 4:
		case 5:
		case 6:
			nextspace = 3;
			break;
		case 7:
		case 8:
		case 9:
			nextspace = 4;
			break;
		}
	}
	
	return nextspace;
}

void Determinedirection(int dir, char arr[60][60], int& stp, int dist, int& p, int& q)
{
	if (dir == 1)
	{
		if (stp == dist - 1 && arr[p - 1][q - 1] != 'O' && arr[p - 1][q - 1] != 'A')
		{
			arr[p - 1][q - 1] = 'B';
		}
		else 
		{
			if (arr[p - 1][q - 1] == 'O' || arr[p - 1][q - 1] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 0 && arr[p - 1][q - 1] != '.')
				{
					p = p + 60;
				}
				arr[p - 1][q - 1] = 'O';
				p--;
				q--;
			}
		}
	}
	if (dir == 2)
	{
		if (stp == dist - 1 && arr[p - 1][q] != 'O' && arr[p - 1][q] != 'A')
		{
			arr[p - 1][q] = 'B';
		}
		else
		{
			if (arr[p - 1][q] == 'O' || arr[p - 1][q] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 0 && arr[p - 1][q] != '.')
				{
					p = p + 60;
				}
				arr[p - 1][q] = 'O';
				p--;
			}
		}
	}
	if (dir == 3)
	{
		if (stp == dist - 1 && arr[p - 1][q + 1] != 'O' && arr[p - 1][q + 1] != 'A')
		{
			arr[p - 1][q + 1] = 'B';
		}
		else
		{
			if (arr[p - 1][q + 1] == 'O' || arr[p - 1][q + 1] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 0 && arr[p - 1][q + 1] != '.')
				{
					p = p + 60;
				}
				arr[p - 1][q + 1] = 'O';
				p--;
				q++;
			}
		}
	}
	if (dir== 4)
	{
		if (stp == dist - 1 && arr[p][q + 1] != 'O' && arr[p][q + 1] != 'A')
		{
			arr[p][q + 1] = 'B';
		}
		else 
		{
			if (arr[p][q + 1] == 'O' || arr[p][q + 1] == 'A')
			{
				stp--;
			}
			else
			{
				arr[p][q + 1] = 'O';
				q++;
			}
		}
	}
	if (dir == 5)
	{
		if (stp == dist - 1 && arr[p + 1][q + 1] != 'O' && arr[p + 1][q + 1] != 'A')
		{
			arr[p + 1][q + 1] = 'B';
		}
		else
		{
			if (arr[p + 1][q + 1] == 'O' || arr[p + 1][q + 1] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 60 && arr[p + 1][q + 1] != '.')
				{
					p = p - 61;
				}
				arr[p + 1][q + 1] = 'O';
				p++;
				q++;
			}
		}
	}
	if (dir == 6)
	{
		if (stp == dist - 1 && arr[p + 1][q] != 'O' && arr[p + 1][q] != 'A')
		{
			arr[p + 1][q] = 'B';
		}
		else
		{
			if (arr[p + 1][q] == 'O' || arr[p + 1][q] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 60 && arr[p + 1][q] != '.')
				{
					p = p - 61;
				}
				arr[p + 1][q] = 'O';
				p++;
			}
		}
	}
	if (dir == 7)
	{
		if (stp == dist - 1 && arr[p + 1][q - 1] != 'O' && arr[p + 1][q - 1] != 'A')
		{
			arr[p + 1][q - 1] = 'B';
		}
		else
		{
			if (arr[p + 1][q - 1] == 'O' || arr[p + 1][q - 1] == 'A')
			{
				stp--;
			}
			else
			{
				if (p == 60 && arr[p + 1][q - 1] != '.')
				{
					p = p - 61;
				}
				arr[p + 1][q - 1] = 'O';
				p++;
				q--;
			}
		}
	}
	if (dir== 8)
	{
		if (stp == dist - 1 && arr[p][q - 1] != 'O' && arr[p][q - 1] != 'A')
		{
			arr[p][q - 1] = 'B';
		}
		else
		{
			if (arr[p][q - 1] == 'O' || arr[p][q - 1] == 'A')
			{
				stp--;
			}
			else
			{
				arr[p][q - 1] = 'O';
				q--;
			}
		}
	}
}

void Printpath(char arr[60][60])
{
	for (int row = 0; row < 60; row++)
	{
		for (int col = 0; col < 60; col++)
		{
			cout << arr[row][col];
		}
		cout << endl;
	}
	cout << endl << endl;
}

