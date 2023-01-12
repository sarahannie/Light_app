# Power Consumption Measurement Application
This application is a tool to measure the specific power consumption of a single application on the computer. It allows the user to select an application from a list of all running processes on the system, displays the power consumption in a graph and the average power consumption per hour.

## Dependencies
This application requires the following dependencies to be installed:

.NET 7
WinForms.DataVisualization

## How to use
Open the application and select the process you want to measure the power consumption from the dropdown list.
The application will start measuring the power consumption and the graph and average consumption textbox will refresh with the new data and points every second.

## How it works
The application uses the Windows Forms UI framework to display a form with a ComboBox that allows the user to select a process from a list of all running processes on the system, a Chart control that displays the power consumption of the selected process in a line graph and a Textbox that displays the average power consumption per hour.

The application uses the PerformanceCounter class to measure the private working set memory usage of the selected process. It is a good approximation of the power consumption of that process.

It will call the NextValue method to get the latest data point and add it to the graph and the textbox.

## Limitations
It is important to note that the private working set memory usage is not an exact measure of power consumption, but it is an approximation of it.

It's also worth mentioning that the power usage may fluctuate due to various factors. Therefore, the measurement may not be 100% accurate.

Additionally, this application doesn't support background applications and power usage from these applications is not included.