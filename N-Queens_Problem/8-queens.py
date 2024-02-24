import random

BOARD_SIZE = 8
POPULATION_SIZE = 100
MUTATION_RATE = 0.1
MAX_GENERATIONS = 500

# Generate a random initial population
def generate_population(size):
    population = []
    for i in range(size):
        individual = random.sample(range(BOARD_SIZE), BOARD_SIZE)
        population.append(individual)
    return population

# Calculate the fitness of a solution (lower is better)
def calculate_fitness(solution):
    conflicts = 0
    for i in range(BOARD_SIZE):
        for j in range(i + 1, BOARD_SIZE):
            if solution[i] == solution[j] or abs(solution[i] - solution[j]) == j - i:
                conflicts += 1
    return conflicts

# Select parents for mating based on tournament selection
def select_parents(population, k=5):
    return random.choice(sorted(population, key=calculate_fitness)[:k])

# Crossover two parents to create a child
def crossover(parent1, parent2):
    crossover_point = random.randint(1, BOARD_SIZE - 1)
    child = parent1[:crossover_point] + parent2[crossover_point:]
    return child

# Mutate a solution by swapping two random queens
def mutate(solution):
    if random.random() < MUTATION_RATE:
        i, j = random.sample(range(BOARD_SIZE), 2)
        solution[i], solution[j] = solution[j], solution[i]

# Main genetic algorithm
def genetic_algorithm():
    population = generate_population(POPULATION_SIZE)

    for generation in range(MAX_GENERATIONS):
        new_population = []

        # Create the next generation
        for i in range(POPULATION_SIZE // 2):
            parent1 = select_parents(population)
            parent2 = select_parents(population)
            child1 = crossover(parent1, parent2)
            child2 = crossover(parent2, parent1)
            mutate(child1)
            mutate(child2)
            new_population.extend([child1, child2])

        population = new_population

        # Check for a solution
        for solution in population:
            if calculate_fitness(solution) == 0:
                return solution, generation + 1

    return None, MAX_GENERATIONS

if __name__ == "__main__":
    solution, generations = genetic_algorithm()

    if solution:
        print(f"Found a solution in {generations} generations:")
        for row in solution:
            formatted_row = ""
            for i in range(BOARD_SIZE):
                if i == row:
                    formatted_row += "Q "
                else:
                    formatted_row += ". "
            print(formatted_row)
    else:
        print("No solution found within the maximum number of generations (being 500).")

