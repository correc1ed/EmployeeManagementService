CREATE TABLE passports (
    id SERIAL PRIMARY KEY,
    type VARCHAR(50) NOT NULL,
    number VARCHAR(50) NOT NULL
);

CREATE TABLE departments (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL
);

CREATE TABLE employees (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    surname VARCHAR(100) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    company_id INT NOT NULL,
    passport_id INT NOT NULL REFERENCES passports(id) ON DELETE CASCADE,
    department_id INT NOT NULL REFERENCES departments(id) ON DELETE CASCADE
);