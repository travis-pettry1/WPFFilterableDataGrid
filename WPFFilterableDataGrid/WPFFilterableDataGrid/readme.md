# WPF Filterable Data Grid

This package allows you to create a data grid that has filterable columns

# Example
```
<Window x:Class="TestApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:TestApp"
        xmlns:filter="clr-namespace:WPFFilterableDataGrid;assembly=WPFFilterableDataGrid"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800"
        x:Name="self">
    <Grid>
        <filter:FilterableDataGrid ItemsSource="{Binding Users}" CanUserAddRows="False" CanUserDeleteRows="False" AutoGenerateColumns="False">
            <filter:FilterableDataGrid.Columns>
                <filter:FilterableDataGridTextColumn Binding="{Binding FirstName}" Header="First Name" />
                <filter:FilterableDataGridTextColumn Binding="{Binding LastName}" Header="Last Name" />
            </filter:FilterableDataGrid.Columns>
        </filter:FilterableDataGrid>
    </Grid>
</Window>
```