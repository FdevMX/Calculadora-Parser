# Scientific Calculator in C#
This scientific calculator is a project developed in C# that combines the use of grammars, parsers, and native C# functionalities. Initially, grammars and parsers were developed to interpret and process mathematical expressions. Subsequently, the calculation logic was implemented using the C# language.

## Features
- Basic arithmetic operations (addition, subtraction, multiplication, and division)
- Advanced operations (powers, roots, logarithms, trigonometric functions, etc.)
- Handling complex expressions with nested parentheses
- History of performed calculations
- Intuitive and user-friendly graphical user interface (GUI)

## Screenshots
![Screenshot 1](https://github.com/FdevMX/Calculadora-Parser/blob/7d9ce352e9df4b20ee7f224dd0725ab23541affe/files/screenshots/image_1.png)
![Screenshot 2](https://github.com/FdevMX/Calculadora-Parser/blob/7d9ce352e9df4b20ee7f224dd0725ab23541affe/files/screenshots/image_2.png)

## Installation
1. Clone this repository to your local machine.
2. Open the solution in Visual Studio or your preferred C# IDE.
3. Build and run the project.

## Resolving Import Errors

To resolve import errors for the following files:

- `CalithaLib.dll`
- `GoldParserEngine.dll`

Follow these steps:

1. Navigate to the project folder and locate the "References" folder.
2. Right-click and select the "Add Reference" option.
3. Choose the "Browse" option and navigate to the cloned repository folder.
4. Locate the "files/dll" folder and import the two existing files.
5. In the "Reference Manager", select the missing "dll" files and click "OK".

If there is an issue with the `gramar_tables.cgt` file, follow these steps:

1. Navigate to the project folder and locate the "bin/Debug" folder.
2. Right-click and select the "Add Existing Item" option.
3. Navigate to the cloned repository folder and locate the "files/gramaticas" folder.
4. Select the "gramar_tables.cgt" file and click "OK".

After following these instructions, the import errors should be resolved.

## Usage
1. Enter a mathematical expression in the calculator's input field.
2. Press the "=" button or the "Enter" key to get the result.
3. Use the corresponding buttons to perform operations or access additional functionalities.
4. Consult the history of performed calculations in the designated area.

## License
This project is licensed under the [GNU General Public License v3.0](LICENSE).