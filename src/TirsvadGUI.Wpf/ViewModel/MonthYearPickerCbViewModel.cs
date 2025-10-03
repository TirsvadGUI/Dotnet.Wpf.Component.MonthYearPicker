using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace TirsvadGUI.Wpf.ViewModel;

/// <summary>
/// ViewModel for a Month/Year picker ComboBox component.
/// Manages selectable years and months within a date range and exposes the selected date.
/// </summary>
public class MonthYearPickerCbViewModel : INotifyPropertyChanged
{
    #region Backing Fields
    private DateTime _beginsAtDate;
    private DateTime _endsAtDate;
    private int _selectedMonth;
    private int _selectedYear;
    private DateTime _selectedDate;
    private bool _firstDayOfMonth;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets the earliest selectable date.
    /// Changing this will update the available years and months.
    /// </summary>
    public DateTime BeginsAtDate
    {
        get => _beginsAtDate;
        set
        {
            if (_beginsAtDate != value)
            {
                _beginsAtDate = value;
                OnPropertyChanged();
                RefreshCollectionsAndSelection();
            }
        }
    }

    /// <summary>
    /// Gets or sets the latest selectable date.
    /// Changing this will update the available years and months.
    /// </summary>
    public DateTime EndsAtDate
    {
        get => _endsAtDate;
        set
        {
            if (_endsAtDate != value)
            {
                _endsAtDate = value;
                OnPropertyChanged();
                RefreshCollectionsAndSelection();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected month index (0-based, relative to the available months for the selected year).
    /// </summary>
    public int SelectedMonth
    {
        get => _selectedMonth;
        set
        {
            if (_selectedMonth != value)
            {
                _selectedMonth = value;
                UpdateSelectedDate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected year.
    /// Changing this will update the available months.
    /// </summary>
    public int SelectedYear
    {
        get => _selectedYear;
        set
        {
            if (_selectedYear != value)
            {
                _selectedYear = value;
                UpdateMonthsCollection();
                UpdateSelectedDate();
            }
        }
    }

    /// <summary>
    /// Gets or sets the currently selected date.
    /// </summary>
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate != value)
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the first day of the month should be selected (true) or the last day (false).
    /// </summary>
    public bool FirstDayOfMonth
    {
        get => _firstDayOfMonth;
        set
        {
            if (_firstDayOfMonth != value)
            {
                _firstDayOfMonth = value;
                UpdateSelectedDate();
            }
        }
    }
    #endregion

    #region ObservableCollection Properties
    /// <summary>
    /// Gets the collection of month names available for the selected year.
    /// </summary>
    public ObservableCollection<string> Months { get; }

    /// <summary>
    /// Gets the collection of years available for selection.
    /// </summary>
    public ObservableCollection<int> Years { get; }
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="MonthYearPickerCbViewModel"/> class.
    /// Sets up the default date range and initializes the selection to the current date.
    /// </summary>
    public MonthYearPickerCbViewModel()
    {
        Months = [];
        Years = [];

        // Set default range
        _beginsAtDate = new DateTime(2000, 1, 1);
        _endsAtDate = DateTime.Now;

        InitializeSelection(DateTime.Now);
    }

    /// <summary>
    /// Initializes the selection and collections based on a reference date.
    /// </summary>
    /// <param name="referenceDate">The date to initialize the selection with.</param>
    private void InitializeSelection(DateTime referenceDate)
    {
        // Clamp referenceDate to the allowed range
        var clamped = referenceDate < _beginsAtDate ? _beginsAtDate : (referenceDate > _endsAtDate ? _endsAtDate : referenceDate);

        UpdateYearsCollection();
        _selectedYear = clamped.Year;
        UpdateMonthsCollection();

        int? startMonth = GetStartMonthForYear(_selectedYear);
        _selectedMonth = startMonth.HasValue ? clamped.Month - startMonth.Value : 0;

        UpdateSelectedDate();
    }

    /// <summary>
    /// Refreshes the years and months collections and updates the selection to match the current date range.
    /// </summary>
    private void RefreshCollectionsAndSelection()
    {
        // Clamp current SelectedDate to new range
        var clamped = _selectedDate < _beginsAtDate ? _beginsAtDate : (_selectedDate > _endsAtDate ? _endsAtDate : _selectedDate);
        UpdateYearsCollection();
        _selectedYear = clamped.Year;
        UpdateMonthsCollection();
        int? startMonth = GetStartMonthForYear(_selectedYear);
        _selectedMonth = startMonth.HasValue ? clamped.Month - startMonth.Value : 0;
        UpdateSelectedDate();
        OnPropertyChanged(nameof(SelectedYear));
        OnPropertyChanged(nameof(SelectedMonth));
    }

    /// <summary>
    /// Updates the <see cref="Months"/> collection based on the currently selected year.
    /// Sets the default selected month to the first available month.
    /// </summary>
    private void UpdateMonthsCollection()
    {
        Months.Clear();
        int? startMonth = GetStartMonthForYear(_selectedYear);
        int? endMonth = GetEndMonthForYear(_selectedYear);

        if (startMonth == null || endMonth == null)
            return;

        var monthNames = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
        for (int month = startMonth.Value; month <= endMonth.Value; month++)
        {
            Months.Add(monthNames[month - 1]);
        }
        OnPropertyChanged(nameof(Months));

        // Always select the first month in the list by default
        if (Months.Count > 0)
        {
            SelectedMonth = 0;
            OnPropertyChanged(nameof(SelectedMonth));
        }
    }

    /// <summary>
    /// Updates the <see cref="Years"/> collection based on the current date range.
    /// </summary>
    private void UpdateYearsCollection()
    {
        Years.Clear();
        for (int year = _beginsAtDate.Year; year <= _endsAtDate.Year; year++)
        {
            Years.Add(year);
        }
        OnPropertyChanged(nameof(Years));
    }

    /// <summary>
    /// Updates the <see cref="SelectedDate"/> property based on the current selection and settings.
    /// </summary>
    private void UpdateSelectedDate()
    {
        if (Years.Count == 0)
            return;
        int minYear = Years[0];
        int maxYear = Years[^1];
        if (_selectedYear < minYear || _selectedYear > maxYear)
            return;
        int? startMonth = GetStartMonthForYear(_selectedYear);
        int? endMonth = GetEndMonthForYear(_selectedYear);
        if (startMonth == null || endMonth == null)
            return;
        if (_selectedMonth < 0 || _selectedMonth >= (endMonth.Value - startMonth.Value + 1))
            return;
        int month = startMonth.Value + _selectedMonth;
        int day = _firstDayOfMonth ? 1 : DateTime.DaysInMonth(_selectedYear, month);
        SelectedDate = new DateTime(_selectedYear, month, day);
    }

    /// <summary>
    /// Gets the first selectable month for a given year.
    /// </summary>
    /// <param name="year">The year to check.</param>
    /// <returns>The first selectable month (1-based), or null if the year is out of range.</returns>
    public int? GetStartMonthForYear(int year)
    {
        if (year < _beginsAtDate.Year || year > _endsAtDate.Year)
            return null;
        return year == _beginsAtDate.Year ? _beginsAtDate.Month : 1;
    }

    /// <summary>
    /// Gets the last selectable month for a given year.
    /// </summary>
    /// <param name="year">The year to check.</param>
    /// <returns>The last selectable month (1-based), or null if the year is out of range.</returns>
    private int? GetEndMonthForYear(int year)
    {
        if (year < _beginsAtDate.Year || year > _endsAtDate.Year)
            return null;
        return year == _endsAtDate.Year ? _endsAtDate.Month : 12;
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
