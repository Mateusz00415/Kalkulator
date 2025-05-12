using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kalkulator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double firstNumber = 0;
    private double secondNumber = 0;
    private string? operationSymbol = null;
    private bool clearDisplayOnNextDigit = false;
    private bool hasUserTypedSecondNumber = false;
    private double? lastSecondNumber = null;


    
    public MainWindow()
    
    {
        InitializeComponent();
    }
    
    private void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        Display.Content = "0";
        firstNumber = 0;
        secondNumber = 0;
        operationSymbol = null;
    }
    
    private void NumberButton_OnClick(object sender, RoutedEventArgs e)
    {
        Button? button = sender as Button;
        string? digit = button?.Content.ToString();

        if (digit == null) return;

        if (clearDisplayOnNextDigit)
        {
            Display.Content = digit;
            clearDisplayOnNextDigit = false;
        }
        else
        {
            string current = Display.Content.ToString();
            if (current == "0")
            {
                if (digit == "0") return;
                Display.Content = digit;
            }
            else
            {
                Display.Content += digit;
            }
        }
        hasUserTypedSecondNumber = true;
    }

    private void PercentButton_OnClick(object sender, RoutedEventArgs e)
    {
        string current = Display.Content.ToString();

        if (!double.TryParse(current, out double value)) return;

        double percentValue = value / 100.0;
        
        Display.Content = percentValue.ToString();
        
        hasUserTypedSecondNumber = true;
    }

    private void OperatorButton_OnClick(object sender, RoutedEventArgs e)
    {
        Button? button = sender as Button;
        string? op = button?.Content.ToString();
        string currentText = Display.Content.ToString();

        if (op == "=")
        {
            if (operationSymbol != null)
            {
                if (hasUserTypedSecondNumber)
                {
                    if (double.TryParse(currentText, out secondNumber))
                    {
                        lastSecondNumber = secondNumber;
                        double result = Calculate();
                        Display.Content = result.ToString();
                        firstNumber = result;
                        hasUserTypedSecondNumber = false;
                    }
                }
                else if (lastSecondNumber != null)
                {
                    secondNumber = (double)lastSecondNumber;
                    double result = Calculate();
                    Display.Content = result.ToString();
                    firstNumber = result;
                }
            }
        }
        else
        {
            if (operationSymbol != null && hasUserTypedSecondNumber)
            {
                if (double.TryParse(currentText, out secondNumber))
                {
                    double result = Calculate();
                    Display.Content = result.ToString();
                    firstNumber = result;
                }
            }
            else
            {
                double.TryParse(currentText, out firstNumber);
            }

            operationSymbol = op;
            clearDisplayOnNextDigit = true;
            hasUserTypedSecondNumber = false;
        }
    }

    private void DotButton_Click(object sender, RoutedEventArgs e)
    {
        if (!Display.Content.ToString().Contains(","))
        {
            Display.Content += ",";
        }
    }
    
    private double Calculate()
    {
        switch (operationSymbol)
        {
            case "-":
                return firstNumber - secondNumber;
            case "+":
                return firstNumber + secondNumber;
            case "*":
                return firstNumber * secondNumber;
            case "/":
                return firstNumber / secondNumber;
        }
        return 0;
    }
    
   
}