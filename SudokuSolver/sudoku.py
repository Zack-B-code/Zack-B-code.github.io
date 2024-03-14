def print_sudoku(board):
    for row in board:
        print(' '.join(map(str, row)))

def is_valid_move(board, row, col, num):
    for i in range(9):
        if board[row][i] == num or board[i][col] == num:
            return False

    start_row, start_col = 3 * (row // 3), 3 * (col // 3)
    for i in range(3):
        for j in range(3):
            if board[start_row + i][start_col + j] == num:
                return False

    return True

def solve_sudoku(board):
    for row in range(9):
        for col in range(9):
            if board[row][col] == 0:
                for num in range(1, 10):
                    if is_valid_move(board, row, col, num):
                        board[row][col] = num
                        if solve_sudoku(board):
                            return True
                        board[row][col] = 0  # Undo the assignment if the solution is not valid
                return False
    return True

def get_user_input():
    print("Enter the Sudoku puzzle as 9 lines of 9 digits (use '0' for empty cells):")
    puzzle = []
    for _ in range(9):
        row = input().strip()
        if len(row) != 9 or not row.isdigit():
            print("Invalid input. Please enter 9 digits for each row.")
            return None
        puzzle.append(list(map(int, row)))
    return puzzle

# Input Sudoku puzzle as a list of lists
sample_sudoku = [
    [0, 0, 4, 0, 0, 0, 0, 0, 0],
    [0, 0, 0, 0, 3, 0, 0, 0, 2],
    [3, 9, 0, 7, 0, 0, 0, 8, 0],
    [4, 0, 0, 0, 0, 9, 0, 0, 1],
    [2, 0, 9, 8, 0, 1, 3, 0, 7],
    [6, 0, 0, 2, 0, 0, 0, 0, 8],
    [0, 1, 0, 0, 0, 8, 0, 5, 3],
    [9, 0, 0, 0, 4, 0, 0, 0, 0],
    [0, 0, 0, 0, 0, 0, 8, 0, 0]
]

print("This is an example of an initial incomplete Sudoku Puzzle (zeros are missing numbers)")
print_sudoku(sample_sudoku)
if solve_sudoku(sample_sudoku):
    print("Here is the example Sudoku solved:")
    print_sudoku(sample_sudoku)
else:
    print("No solution exists.")



input_sudoku = get_user_input()

if input_sudoku and solve_sudoku(input_sudoku):
    print("Here is the solved version of the Sudoku you input:")
    print_sudoku(input_sudoku)
else:
    print("No solution exists or invalid input.")
