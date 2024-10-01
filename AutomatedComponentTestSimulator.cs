using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutomatedComponentTester
{
    /// <summary>
    /// Enumerates the types of electronic components that can be tested.
    /// </summary>
    public enum ComponentType
    {
        Resistor,
        Capacitor,
        Inductor,
        Transistor
    }

    /// <summary>
    /// Represents an electronic component with its characteristics and test results.
    /// </summary>
    public class Component
    {
        /// <summary>Gets or sets the type of the component.</summary>
        public ComponentType Type { get; set; }

        /// <summary>Gets or sets the nominal value of the component.</summary>
        public double NominalValue { get; set; }

        /// <summary>Gets or sets the measured actual value of the component after testing.</summary>
        public double ActualValue { get; set; }

        /// <summary>Gets or sets the tolerance of the component as a decimal (e.g., 0.05 for 5%).</summary>
        public double Tolerance { get; set; }
    }

    /// <summary>
    /// Provides functionality to simulate testing of electronic components.
    /// </summary>
    public class Tester
    {
        private Random _random = new Random();

        /// <summary>
        /// Simulates the testing of a batch of components asynchronously.
        /// </summary>
        /// <param name="batch">A list of components to be tested.</param>
        /// <returns>A task that represents the asynchronous operation, containing the list of tested components.</returns>
        public async Task<List<Component>> TestBatch(List<Component> batch)
        {
            List<Task<Component>> testTasks = new List<Task<Component>>();

            foreach (var component in batch)
            {
                testTasks.Add(TestComponent(component));
            }

            return await Task.WhenAll(testTasks);
        }

        /// <summary>
        /// Simulates the testing of a single component.
        /// </summary>
        /// <param name="component">The component to be tested.</param>
        /// <returns>A task that represents the asynchronous operation, containing the tested component.</returns>
        private async Task<Component> TestComponent(Component component)
        {
            // Simulate testing time with a random delay
            await Task.Delay(_random.Next(100, 500));

            // Simulate measurement with random variation within the component's tolerance
            double variation = ((_random.NextDouble() * 2) - 1) * component.Tolerance * component.NominalValue;
            component.ActualValue = component.NominalValue + variation;

            return component;
        }

        /// <summary>
        /// Analyzes and displays the results of the component testing.
        /// </summary>
        /// <param name="testedBatch">A list of components that have been tested.</param>
        public void AnalyzeResults(List<Component> testedBatch)
        {
            int passCount = 0;
            int failCount = 0;

            foreach (var component in testedBatch)
            {
                bool withinTolerance = Math.Abs(component.ActualValue - component.NominalValue) <= 
                                       (component.Tolerance * component.NominalValue);

                if (withinTolerance)
                    passCount++;
                else
                    failCount++;

                Console.WriteLine($"Component: {component.Type}, " +
                                  $"Nominal: {component.NominalValue}, " +
                                  $"Actual: {component.ActualValue:F2}, " +
                                  $"Result: {(withinTolerance ? "PASS" : "FAIL")}");
            }

            Console.WriteLine($"\nBatch Summary:");
            Console.WriteLine($"Total: {testedBatch.Count}, Pass: {passCount}, Fail: {failCount}");
            Console.WriteLine($"Yield: {(double)passCount / testedBatch.Count:P2}");
        }
    }

    /// <summary>
    /// Entry point for the Automated Component Tester application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments (not used).</param>
        static async Task Main(string[] args)
        {
            Tester tester = new Tester();
            List<Component> batch = new List<Component>
            {
                new Component { Type = ComponentType.Resistor, NominalValue = 100, Tolerance = 0.05 },
                new Component { Type = ComponentType.Capacitor, NominalValue = 10e-6, Tolerance = 0.1 },
                new Component { Type = ComponentType.Inductor, NominalValue = 1e-3, Tolerance = 0.05 },
                new Component { Type = ComponentType.Transistor, NominalValue = 50, Tolerance = 0.1 }
            };

            Console.WriteLine("Starting Automated Component Test...");
            var testedBatch = await tester.TestBatch(batch);
            tester.AnalyzeResults(testedBatch);
        }
    }
}