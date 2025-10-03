using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TirsvadGUI.Wpf.ViewModel;

namespace TirsvadGUI.Wpf.Components;

/// <summary>
/// Interaction logic for MonthYearPickerCb.xaml
/// </summary>
public partial class MonthYearPickerCb : UserControl
{
    private readonly MonthYearPickerCbViewModel vm;

    #region Dependency Properties
    public static readonly DependencyProperty BeginsAtDateProperty =
        DependencyProperty.Register(
            nameof(BeginsAtDate),
            typeof(DateTime),
            typeof(MonthYearPickerCb),
            new PropertyMetadata(new DateTime(1990, 1, 1)));

    public DateTime BeginsAtDate
    {
        get => (DateTime)GetValue(BeginsAtDateProperty);
        set
        {
            SetValue(BeginsAtDateProperty, value);
            //vm.BeginsAtDate = this.BeginsAtDate;
        }
    }
    public static readonly DependencyProperty EndsAtDateProperty =
        DependencyProperty.Register(
            nameof(EndsAtDate),
            typeof(DateTime),
            typeof(MonthYearPickerCb),
            new PropertyMetadata(new DateTime(1990, 1, 1)));

    public DateTime EndsAtDate
    {
        get => (DateTime)GetValue(EndsAtDateProperty);
        set
        {
            SetValue(EndsAtDateProperty, value);
            //vm.EndsAtDate = this.EndsAtDate;
        }
    }

    // true: first day of month, false: last day of month
    public static readonly DependencyProperty FirstDayOfMonthProperty =
        DependencyProperty.Register(
            nameof(FirstDayOfMonth),
            typeof(bool),
            typeof(MonthYearPickerCb),
            new PropertyMetadata(true));

    public bool FirstDayOfMonth
    {
        get => (bool)GetValue(FirstDayOfMonthProperty);
        set => SetValue(FirstDayOfMonthProperty, value);
    }

    // true: first day of month, false: last day of month
    public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register(
            nameof(SelectedDate),
            typeof(DateTime),
            typeof(MonthYearPickerCb),
            new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged));

    public DateTime SelectedDate
    {
        get => (DateTime)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MonthYearPickerCb control && control.DataContext is MonthYearPickerCbViewModel vm)
        {
            var date = (DateTime)e.NewValue;
            vm.SelectedYear = date.Year;
            int? startMonth = vm.GetStartMonthForYear(date.Year);
            if (startMonth.HasValue)
                vm.SelectedMonth = date.Month - startMonth.Value;
            else
                vm.SelectedMonth = 0;
            vm.SelectedDate = date;
        }
    }
    #endregion

    #region properties
    public ObservableCollection<string> Months
    {
        get
        {
            var months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                .Where(m => !string.IsNullOrEmpty(m))
                .ToList();
            return new ObservableCollection<string>(months);
        }

    }

    public ObservableCollection<int> Years
    {
        get
        {
            var years = new ObservableCollection<int>();
            for (int year = BeginsAtDate.Year; year <= EndsAtDate.Year; year++)
            {
                years.Add(year);
            }
            return years;
        }
    }

    #endregion

    public MonthYearPickerCb()
    {
        InitializeComponent();
        vm = new MonthYearPickerCbViewModel();
        this.DataContext = vm;
        this.Loaded += MonthYearPickerCb_Loaded;
    }

    private void MonthYearPickerCb_Loaded(object sender, RoutedEventArgs e)
    {
        vm.BeginsAtDate = this.BeginsAtDate;
        vm.EndsAtDate = this.EndsAtDate;
        vm.SelectedDate = this.SelectedDate;
        vm.FirstDayOfMonth = this.FirstDayOfMonth;
    }
}
