Zack Bahn
Spam Filter Program

- This goal of this program is to take in a large dataset of messages that are labelled as spam or ham, to then learn from 
and be able to clssify incoming messages as ham or spam.
- For this program to work, I first seperated the SMS_Dataset file into two files, named spam_messages and ham_messages.
- I used those files to calculate the overall probabilities that any given line is spam or ham. Those values are outputted first.
- For the sake of testing and showing an example, I wrote code that allows the user to imput any word or phrase and output 
a few probabilities related to that input.
- Next, I created a testing set, for which I tried a few different sizes so that the code would run somewhat quickly.
- I used a function to classify each message in the testing set as either spam or ham, for which i called the calculate probability function
that was used to produce the example.
- Upon classifying each line, I added each one to a newly created classified_messages.txt file.
- I compared each line in the new classified_messages.txt file to the original SMS_Dataset.txt file to compute several values.
- The calculated values are then finally outputted at the end of the program. 

You can compile / run this program by using this command: python3 spamfilter.py
- Make sure the SMS_Dataset.txt file is in the same directory as the .py file.
- This version of the code uses a testing set of 100, but this can be changed within the for loop located on line 100.

- Thanks for reading!



