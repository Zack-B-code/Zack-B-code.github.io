import random

POPULATION_SIZE = 100
MUTATION_RATE = 0.1
MAX_GENERATIONS = 500

def is_safe(solution):
    n = len(solution)
    for i in range(n):
        for j in range(i + 1, n):
            if solution[i] == solution[j] or abs(solution[i] - solution[j]) == j - i:
                return False
    return True

def generate_population(size, n):
    population = []
    for i in range(size):
        individual = random.sample(range(n), n)
        population.append(individual)
    return population

def calculate_fitness(solution):
    conflicts = 0
    n = len(solution)
    for i in range(n):
        for j in range(i + 1, n):
            if solution[i] == solution[j] or abs(solution[i] - solution[j]) == j - i:
                conflicts += 1
    return conflicts

def select_parents(population, k=5):
    return random.choice(sorted(population, key=calculate_fitness)[:k])

def crossover(parent1, parent2):
    n = len(parent1)
    crossover_point = random.randint(1, n - 1)
    child = parent1[:crossover_point] + parent2[crossover_point:]
    return child

def mutate(solution, mutation_rate):
    n = len(solution)
    if random.random() < mutation_rate:
        i, j = random.sample(range(n), 2)
        solution[i], solution[j] = solution[j], solution[i]

def genetic_algorithm(n, population_size, mutation_rate, max_generations):
    population = generate_population(POPULATION_SIZE, n)

    for generation in range(max_generations):
        new_population = []

        for i in range(population_size // 2):
            parent1 = select_parents(population)
            parent2 = select_parents(population)
            child1 = crossover(parent1, parent2)
            child2 = crossover(parent2, parent1)
            mutate(child1, mutation_rate)
            mutate(child2, mutation_rate)
            new_population.extend([child1, child2])

        population = new_population

        for solution in population:
            if is_safe(solution):
                return solution, generation + 1

    return None, max_generations

if __name__ == "__main__":
    n = int(input("Enter the board size (n): "))

    solution, generations = genetic_algorithm(n, POPULATION_SIZE, MUTATION_RATE, MAX_GENERATIONS)

    if solution:
        print(f"Found a solution in {generations} generations:")
        for row in solution:
            formatted_row = ""
            for i in range(n):
                if i == row:
                    formatted_row += "Q "
                else:
                    formatted_row += ". "
            print(formatted_row)
    else:
        print("No solution found within the maximum number of generations (being 500).")
