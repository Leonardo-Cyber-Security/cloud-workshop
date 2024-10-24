CREATE DATABASE mydb;

\c mydb;

CREATE TABLE Items (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255),
  description TEXT,
  quantity INT
);

-- Aggiunta di dati iniziali
INSERT INTO Items (name, description, quantity) VALUES 
('Item 1', 'Descrizione del primo item', 10),
('Item 2', 'Descrizione del secondo item', 20),
('Item 3', 'Descrizione del terzo item', 30);
