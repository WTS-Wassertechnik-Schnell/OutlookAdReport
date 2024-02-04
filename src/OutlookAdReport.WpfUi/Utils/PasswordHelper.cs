using System.Windows;
using System.Windows.Controls;

namespace OutlookAdReport.WpfUi.Utils;

/// <summary> A password helper.</summary>
public static class PasswordHelper
{
    /// <summary> (Immutable) A Dependency Property for the password property.</summary>
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    /// <summary> (Immutable) A Dependency Property for the attach property.</summary>
    public static readonly DependencyProperty AttachProperty =
        DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, Attach));

    /// <summary> (Immutable) A Dependency Property for the is updating property.</summary>
    private static readonly DependencyProperty IsUpdatingProperty =
        DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
            typeof(PasswordHelper));


    /// <summary> Sets an attach.</summary>
    /// <param name="dp">    The dp. </param>
    /// <param name="value"> True to value. </param>
    public static void SetAttach(DependencyObject dp, bool value)
    {
        dp.SetValue(AttachProperty, value);
    }

    /// <summary> Gets an attach.</summary>
    /// <param name="dp"> The dp. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    public static bool GetAttach(DependencyObject dp)
    {
        return (bool)dp.GetValue(AttachProperty);
    }

    /// <summary> Gets a password.</summary>
    /// <param name="dp"> The dp. </param>
    /// <returns> The password.</returns>
    public static string GetPassword(DependencyObject dp)
    {
        return (string)dp.GetValue(PasswordProperty);
    }

    /// <summary> Sets a password.</summary>
    /// <param name="dp">    The dp. </param>
    /// <param name="value"> True to value. </param>
    public static void SetPassword(DependencyObject dp, string value)
    {
        dp.SetValue(PasswordProperty, value);
    }

    /// <summary> Gets is updating.</summary>
    /// <param name="dp"> The dp. </param>
    /// <returns> True if it succeeds, false if it fails.</returns>
    private static bool GetIsUpdating(DependencyObject dp)
    {
        return (bool)dp.GetValue(IsUpdatingProperty);
    }

    /// <summary> Sets is updating.</summary>
    /// <param name="dp">    The dp. </param>
    /// <param name="value"> True to value. </param>
    private static void SetIsUpdating(DependencyObject dp, bool value)
    {
        dp.SetValue(IsUpdatingProperty, value);
    }

    /// <summary> Raises the dependency property changed event.</summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Event information to send to registered event handlers. </param>
    private static void OnPasswordPropertyChanged(DependencyObject sender,
        DependencyPropertyChangedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        passwordBox!.PasswordChanged -= PasswordChanged;

        if (!GetIsUpdating(passwordBox)) passwordBox.Password = (string)e.NewValue;
        passwordBox.PasswordChanged += PasswordChanged;
    }

    /// <summary> Attaches.</summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Dependency property changed event information. </param>
    private static void Attach(DependencyObject sender,
        DependencyPropertyChangedEventArgs e)
    {
        if (sender is not PasswordBox passwordBox)
            return;

        if ((bool)e.OldValue) passwordBox.PasswordChanged -= PasswordChanged;

        if ((bool)e.NewValue) passwordBox.PasswordChanged += PasswordChanged;
    }

    /// <summary> Password changed.</summary>
    /// <param name="sender"> The sender. </param>
    /// <param name="e">      Routed event information. </param>
    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
        var passwordBox = sender as PasswordBox;
        SetIsUpdating(passwordBox!, true);
        SetPassword(passwordBox!, passwordBox!.Password);
        SetIsUpdating(passwordBox, false);
    }
}