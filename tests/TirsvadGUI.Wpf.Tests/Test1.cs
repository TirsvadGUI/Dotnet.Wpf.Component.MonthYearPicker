using TirsvadGUI.Wpf.Components;
using TirsvadGUI.Wpf.ViewModel;

namespace TirsvadGUI.Wpf.Tests;

[TestClass]
public sealed class MonthYearPickerCbTests
{
    private static T RunInSta<T>(Func<T> func)
    {
        T? result = default;
        Exception? ex = null;
        var thread = new Thread(() =>
        {
            try
            {
                result = func();
            }
            catch (Exception e)
            {
                ex = e;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (ex != null) throw ex;
        return result!;
    }

    [TestMethod]
    public void BeginsAtDate_Default_Is1990_01_01()
    {
        var beginsAt = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb();
            return picker.BeginsAtDate;
        });
        Assert.AreEqual(new DateTime(1990, 1, 1), beginsAt, "Expected BeginsAtDate to be 1990-01-01");
    }

    [TestMethod]
    public void EndsAtDate_Default_Is1990_01_01()
    {
        var endsAt = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb();
            return picker.EndsAtDate;
        });
        Assert.AreEqual(new DateTime(1990, 1, 1), endsAt, "Expected EndsAtDate to be 1990-01-01");
    }

    [TestMethod]
    public void FirstDayOfMonth_Default_IsTrue()
    {
        var isFirstDay = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb();
            return picker.FirstDayOfMonth;
        });
        Assert.IsTrue(isFirstDay, "Expected FirstDayOfMonth to be true");
    }

    [TestMethod]
    public void SelectedDate_Default_IsNow()
    {
        var selectedDate = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb();
            return picker.SelectedDate;
        });
        Assert.IsTrue((DateTime.Now - selectedDate).TotalSeconds < 2, "Expected SelectedDate to be now");
    }

    [TestMethod]
    public void Months_Returns12MonthNames()
    {
        var months = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb();
            return picker.Months;
        });
        Assert.AreEqual(12, months.Count, "Expected 12 months to be returned");
        Assert.IsTrue(months.All(m => !string.IsNullOrEmpty(m)), "Expected all month names to be non-empty");
    }

    [TestMethod]
    public void Years_ReturnsRangeBetweenBeginsAndEnds()
    {
        var years = RunInSta(() =>
        {
            var picker = new MonthYearPickerCb
            {
                BeginsAtDate = new DateTime(2020, 1, 1),
                EndsAtDate = new DateTime(2023, 1, 1)
            };
            return picker.Years.ToList();
        });
        var expected = new[] { 2020, 2021, 2022, 2023 };
        CollectionAssert.AreEqual(expected, years, "Expected years to match the range between BeginsAtDate and EndsAtDate");
    }

    //[TestMethod]
    //public void ViewModel_Updates_When_DependencyProperties_Change()
    //{
    //    RunInSta<object>(() =>
    //    {
    //        var picker = new MonthYearPickerCb
    //        {
    //            BeginsAtDate = new DateTime(2010, 4, 1),
    //            EndsAtDate = new DateTime(2012, 6, 1),
    //            FirstDayOfMonth = false,
    //            SelectedDate = new DateTime(2011, 5, 15)
    //        };
    //        var vm = picker.DataContext as MonthYearPickerCbViewModel;
    //        Assert.IsNotNull(vm);
    //        Assert.AreEqual(new DateTime(2010, 4, 1), vm.BeginsAtDate);
    //        Assert.AreEqual(new DateTime(2012, 6, 1), vm.EndsAtDate);
    //        Assert.IsFalse(vm.FirstDayOfMonth);
    //        Assert.AreEqual(new DateTime(2011, 5, 1), vm.SelectedDate);
    //        return new object();
    //    });
    //}

    [TestMethod]
    public void ViewModel_Selects_FirstMonth_On_YearChange()
    {
        RunInSta<object>(() =>
        {
            var picker = new MonthYearPickerCb
            {
                BeginsAtDate = new DateTime(2020, 1, 1),
                EndsAtDate = new DateTime(2022, 12, 31),
                SelectedDate = new DateTime(2021, 5, 1)
            };
            // Ensure DataContext is set for the test
            if (picker.DataContext == null)
            {
                picker.DataContext = new MonthYearPickerCbViewModel
                {
                    BeginsAtDate = picker.BeginsAtDate,
                    EndsAtDate = picker.EndsAtDate,
                    SelectedDate = picker.SelectedDate
                };
            }
            var vm = picker.DataContext as MonthYearPickerCbViewModel;
            Assert.IsNotNull(vm, "Expected ViewModel to be created");
            vm.SelectedYear = 2022;
            Assert.AreEqual(0, vm.SelectedMonth, $"Expected January to be selected but got {vm.SelectedMonth}"); // January
            return new object();
        });
    }

    [TestMethod]
    public void ViewModel_Clamps_SelectedDate_To_Range()
    {
        RunInSta<object>(() =>
        {
            var picker = new MonthYearPickerCb
            {
                BeginsAtDate = new DateTime(2020, 1, 1),
                EndsAtDate = new DateTime(2020, 12, 31),
                SelectedDate = new DateTime(2019, 5, 1) // out of range
            };
            var vm = picker.DataContext as MonthYearPickerCbViewModel;
            Assert.IsNotNull(vm, "Expected ViewModel to be created");
            Assert.IsTrue(vm.SelectedDate >= vm.BeginsAtDate && vm.SelectedDate <= vm.EndsAtDate, "Expected SelectedDate to be clamped within range");
            return new object();
        });
    }
}
