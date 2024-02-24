import os
import math

# Split the dataset into spam and ham files
input_file = 'SMSSpamCollection.txt'
spam_file = 'spam_messages.txt'
ham_file = 'ham_messages.txt'

with open(input_file, 'r') as infile, open(spam_file, 'w') as spam_outfile, open(ham_file, 'w') as ham_outfile:
    for line in infile:
        if line.startswith('spam\t'):
            spam_outfile.write(line)
        elif line.startswith('ham\t'):
            ham_outfile.write(line)

# Calculate the probabilities of spam and ham messages
total_messages = 0
total_spam = 0
total_ham = 0

with open(input_file, 'r') as infile:
    for line in infile:
        total_messages += 1

with open(spam_file, 'r') as spamfile:
    for line in spamfile:
        total_spam += 1

with open(ham_file, 'r') as hamfile:
    for line in hamfile:
        total_ham += 1

p_spam = total_spam / total_messages
p_ham = total_ham / total_messages

print(f'The total amount of messages to be classified are: {total_messages}')
print(f'The total amount of spam messages are: {total_spam}')
print(f'So the probability that any message is spam is: {p_spam}')
print(f'The total amount of ham messages are: {total_ham}')
print(f'So the probability that any message is ham is: {p_ham}')



# Calculate probabilities using Naiive Bayesian approach
def calculate_probability_given_input(input_message, is_spam=True):
    if is_spam:
        total_given_type = total_spam
        total_other_type = total_ham
    else:
        total_given_type = total_ham
        total_other_type = total_spam

    input_words = input_message.split()  # Split input into individual words

    p_input_given_type = 1.0  # Initialize with 1.0 for Laplace Smoothing
    for word in input_words:
        # Calculate the probability of each word given the type (spam or ham) with Laplace Smoothing
        p_word_given_type = 1  # Initialize word count at 1 for Laplace Smoothing
        with open(spam_file if is_spam else ham_file, 'r') as file:
            for line in file:
                if word in line:
                    p_word_given_type += 1

        p_word_given_type /= (total_given_type + 2)  # Add 2 to the denominator for Laplace Smoothing

        p_input_given_type *= p_word_given_type

    p_type_given_input = (p_input_given_type * (total_given_type / total_messages)) / (
        (p_input_given_type * (total_given_type / total_messages)) + (1 - p_input_given_type) * (total_other_type / total_messages))

    return p_type_given_input, p_input_given_type

# Example: User input and output probabilities
user_input = input("Enter a word or phrase: ")
spam_given_input, input_given_spam = calculate_probability_given_input(user_input, is_spam=True)
ham_given_input, input_given_ham = calculate_probability_given_input(user_input, is_spam=False)

print(f"Probability that a message is spam given it contains '{user_input}': {spam_given_input:.4f}")
print(f"Probability that a message is ham given it contains '{user_input}': {ham_given_input:.4f}")
print(f"Probability that '{user_input}' is in a spam message: {input_given_spam:.4f}")
print(f"Probability that '{user_input}' is in a ham message: {input_given_ham:.4f}")

# Testing set code

# Function to classify a message as spam or ham
def classify_message(message, is_spam):
    p_spam_given_message, _ = calculate_probability_given_input(message, is_spam=True) # _ = because we don't need second val function returnes
    p_ham_given_message, _ = calculate_probability_given_input(message, is_spam=False)
    
    if p_spam_given_message > p_ham_given_message:
        label = 'spam'
    else:
        label = 'ham'
    
    return f'{label}\t{message}'

# Load the first bunch of lines from the original dataset as the testing set
testing_set = []
with open(input_file, 'r') as infile:
    for y in range(100):
        line = infile.readline().strip()
        label, message = line.split('\t', 1)
        testing_set.append((label, message))

# Classify each message in the testing set
classified_messages = [classify_message(message, label == 'spam') for label, message in testing_set]

# Save the classified messages to a file
output_file = 'classified_messages.txt'
with open(output_file, 'w') as outfile:
    outfile.write('\n'.join(classified_messages))


# Calculate values for the report

# Load the classified messages from the file
classified_messages = []
with open('classified_messages.txt', 'r') as infile:
    classified_messages = [line.strip() for line in infile]

# Load the original messages with labels
original_messages = []
with open(input_file, 'r') as infile:
    original_messages = [line.strip() for line in infile]

# Create dictionaries to count the matches and mismatches for spam and ham
spam_matches = 0
spam_mismatches = 0
ham_matches = 0
ham_mismatches = 0

for i in range(len(classified_messages)):
    classified_label, _ = classified_messages[i].split('\t', 1)
    original_label, _ = original_messages[i].split('\t', 1)
    
    if classified_label == 'spam' and original_label == 'spam':
        spam_matches += 1
    elif classified_label == 'spam' and original_label == 'ham':
        spam_mismatches += 1
    elif classified_label == 'ham' and original_label == 'ham':
        ham_matches += 1
    elif classified_label == 'ham' and original_label == 'spam':
        ham_mismatches += 1

# Calculate spam and ham precision
spam_precision = spam_matches / (spam_matches + spam_mismatches)
spam_recall = spam_matches / (spam_matches + ham_mismatches)
ham_precision = ham_matches / (ham_matches + ham_mismatches)
ham_recall = ham_matches / (ham_matches + spam_mismatches)
spam_fscore = 2 * (spam_precision * spam_recall) / (spam_precision + spam_recall)
ham_fscore = 2 * (ham_precision * ham_recall) / (ham_precision + spam_recall)
accuracy = (spam_recall + ham_recall) / 2


print(f"Spam Precision: {spam_precision:.4f}")
print(f"Spam Recall: {spam_recall:.4f}")
print(f"Ham Precision: {ham_precision:.4f}")
print(f"Ham Recall: {ham_recall:.4f}")
print(f"Spam F-Score: {spam_fscore:.4f}")
print(f"Ham F-Score: {ham_fscore:.4f}")
print(f"Accuracy: {accuracy:.4f}")
