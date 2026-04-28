# Personalregister

A console application for managing an employee register at a small restaurant.

## Features

- View a list of active employees
- Add new employees
- Edit employee name and salary
- Soft-delete employees (data preserved for the session)

## Running the app

dotnet run

## Design

See [docs/PRD.md](docs/PRD.md) for the full product requirements.

## Notes

- Data is in-memory only — no persistence between runs
- All input is validated at the console boundary