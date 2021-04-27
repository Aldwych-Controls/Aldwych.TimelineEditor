[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]


<br />
<p align="center">

  <h3 align="center">🎞️ Aldwych.TimelineEditor</h3>

  <p align="center">
    A multitrack timeline control, inspired by Logic Pro X.
    <br />    
    <a href="https://aldwych-controls.gitbook.io/ui-controls/"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/issues">Report Bug</a>
    ·
    <a href="https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/issues">Request Feature</a>
  </p>
</p>






## Important Note
This is a **work in progress** and is not anywhere close to finished. Consider it a **experimental** sample rather than a ready to use control. 

I've lots of work to do before I commit to an API and thus things are a little disjointed at the moment. As it stands, this is the result of just a single afternoon of hacking.

I will continue to work on this as and when I find time as I need this control for an app, but I have no timeline (pun unintended) for when this might be. 

# Example usage to use 

## Models

**TrackItem**
```csharp
var clip1 = new TrackItem() 
{ 
    Title = "Test 1", 
    Start = TimeSpan.FromSeconds(10), 
    End = TimeSpan.FromSeconds(20), 
    IsSelected = true 
};
```

**ViewModel**
```csharp
public ObservableCollection<TrackItem> Clips { get; private set; }

public MyViewModel()
{
    Clips = new ObservableCollection<TrackItem>();

    //Create clip(s)

    Clips.Add(clip1);
}
```



Almost all the controls are lookless and thus require some styling in order to render. 

### Styles
```xml
<Window.Styles>

<!-- Track Item (clip) Container -->
<Style Selector="te|TimelinePanel">
      <Setter Property="VerticalAlignment" Value="Top"/>
      <Setter Property="SelectionMode" Value="Multiple"/>
      <Setter Property="Template">
        <ControlTemplate>
          <Border Name="border" BorderBrush="{TemplateBinding BorderBrush}"
                  BorderThickness="{TemplateBinding BorderThickness}">
            <ScrollViewer Name="PART_ScrollViewer"
                          Background="{TemplateBinding Background}"
                          HorizontalScrollBarVisibility="{TemplateBinding (ScrollViewer.HorizontalScrollBarVisibility)}"
                          VerticalScrollBarVisibility="{TemplateBinding (ScrollViewer.VerticalScrollBarVisibility)}">
              <ItemsPresenter Name="PART_ItemsPresenter"
                              Items="{TemplateBinding Items}"
                              ItemsPanel="{TemplateBinding ItemsPanel}"
                              ItemTemplate="{TemplateBinding ItemTemplate}"
                              Margin="{TemplateBinding Padding}"
                              VirtualizationMode="{TemplateBinding VirtualizationMode}"/>
            </ScrollViewer>
          </Border>
        </ControlTemplate>
      </Setter>
    </Style>

    <!-- Track Row -->
    <Style Selector="te|Track">
      <Setter Property="VerticalAlignment" Value="Top"/>
      <Setter Property="SelectionMode" Value="Multiple"/>
      <Setter Property="Template">
        <ControlTemplate>
          <Border Name="border" BorderBrush="{TemplateBinding BorderBrush}"
                  BorderThickness="{TemplateBinding BorderThickness}">
            <ScrollViewer Name="PART_ScrollViewer"
                          Background="{TemplateBinding Background}"
                          HorizontalScrollBarVisibility="{TemplateBinding (ScrollViewer.HorizontalScrollBarVisibility)}"
                          VerticalScrollBarVisibility="{TemplateBinding (ScrollViewer.VerticalScrollBarVisibility)}">
              <ItemsPresenter Name="PART_ItemsPresenter"
                              Items="{TemplateBinding Items}"
                              ItemsPanel="{TemplateBinding ItemsPanel}"
                              ItemTemplate="{TemplateBinding ItemTemplate}"
                              Margin="{TemplateBinding Padding}"                              
                              VirtualizationMode="{TemplateBinding VirtualizationMode}"/>
            </ScrollViewer>
          </Border>
        </ControlTemplate>
      </Setter>
    </Style>

    <!-- Track Item / Clip style -->
    <Style Selector="te|TrackItem">
      <Setter Property="Background" Value="#428418"/>
      <Setter Property="MinWidth" Value="20"/>
      <Setter Property="MinHeight" Value="60"/>
      <Setter Property="Margin" Value="0,0.5"/>
      <Setter Property="Template">
        <ControlTemplate>
        <Border Name="PART_Border"
                CornerRadius="5"
                BorderThickness="2"
                ClipToBounds="True">
          <Panel>
            <ContentPresenter Name="PART_ContentPresenter"
                  Background="{TemplateBinding Background}"
                  BorderBrush="{TemplateBinding BorderBrush}"
                  CornerRadius="4"
                  BorderThickness="{TemplateBinding BorderThickness}"
                  ContentTemplate="{TemplateBinding ContentTemplate}"
                  Content="{TemplateBinding Content}"
                  Padding="{TemplateBinding Padding}"
                  VerticalContentAlignment="{TemplateBinding VerticalContentAlignment}"
                  HorizontalContentAlignment="{TemplateBinding HorizontalContentAlignment}" />
            
            <TextBlock Text="{TemplateBinding Title}" 
                       FontWeight="Medium" 
                       Margin="5,3" 
                       FontSize="12"
                       Foreground="White"
                       ToolTip.Tip="{TemplateBinding Title}"/>
            
          </Panel>
        </Border>
        </ControlTemplate>
      </Setter>
    </Style>

    <!-- Track Item default border -->
    <Style Selector="te|TrackItem /template/ Border#PART_Border">
      <Setter Property="BorderBrush" Value="Black" />
    </Style>

    <!-- Track Item selected border -->
    <Style Selector="te|TrackItem:selected /template/ Border#PART_Border">
      <Setter Property="BorderBrush" Value="Blue" />
    </Style>
    
  </Window.Styles>
```

### Create a TimelineView

```xml
    <ScrollViewer HorizontalScrollBarVisibility="Visible" Background="Transparent" AllowAutoHide="False" >
      <Grid RowDefinitions="Auto, *">

        <Border Grid.Row="0" Height="45" BorderBrush="Black" BorderThickness="0,0,0,2">
          
          <te:TimeRuler  Foreground="#ACACAC"
                         SplitBrush="#1A1A1A"
                         VerticalLineBrush="#3A3A3A"
                         VerticalLineThickness="1"
                         Background="Transparent"
                         TicksPerInterval="60"
                         Width="{Binding #PART_TimelinePanel.Width}"
                         MarkerCount="60"
                         MarkerSpacing="120"/>
          
        </Border>
      <Panel Grid.Row="1">
        <te:TimelineGrid VerticalLineBrush="#3A3A3A"
                         HorizontalLineBrush="#1D1D1D"
                         VerticalLineThickness="1"
                         HorizontalLineThickness="2"
                         Background="#232323"
                         TicksPerInterval="60"
                         Width="{Binding #PART_TimelinePanel.Width}"
                         MarkerCount="60"
                         MarkerSpacing="20"
                         TrackHeight="60"/>

        <te:TimelinePanel Name="PART_TimelinePanel"
                          ZoomLevel="{Binding ZoomLevel, Mode=OneWay}" 
                          Duration="{Binding Duration}">
          <te:Track Items="{Binding Clips}" SelectedItems="{Binding SelectedItems}">
            <te:Track.ItemTemplate>
              <DataTemplate>
                <te:TrackItem Title="{Binding}"
                              Start="{Binding}"
                              End="{Binding}"/>
              </DataTemplate>
            </te:Track.ItemTemplate>
          </te:Track>


        </te:TimelinePanel>
      </Panel>
      </Grid>
    </ScrollViewer>
```
---

## Problems or Suggestions
[Open an issue here](../..//issues)

## Contributors
|        ![Photo](https://avatars.githubusercontent.com/u/552074?s=160&v=4)|  
|:----------------------------------------------:|
|                 **Mike James** - Author                
|  [GitHub](https://github.com/MikeCodesDotNet)  | 
| [Twitter](https://twitter.com/MikeCodesDotNet) | 
        
---
## License
Licensed under MIT see License file.



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/Aldwych-Controls/Aldwych.TimelineEditor?style=for-the-badge
[contributors-url]: https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/graphs/contributors

[forks-shield]: https://img.shields.io/github/forks/Aldwych-Controls/Aldwych.TimelineEditor?style=for-the-badge
[forks-url]: https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/network/members

[stars-shield]: https://img.shields.io/github/stars/Aldwych-Controls/Aldwych.TimelineEditor?style=for-the-badge
[stars-url]: https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/stargazers

[issues-shield]: https://img.shields.io/github/issues/Aldwych-Controls/Aldwych.TimelineEditor?style=for-the-badge
[issues-url]: https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/issues

[license-shield]: https://img.shields.io/github/license/Aldwych-Controls/Aldwych.TimelineEditor?style=for-the-badge
[license-url]: https://github.com/Aldwych-Controls/Aldwych.TimelineEditor/blob/master/LICENSE.txt

[sample-app-screenshot]: assets/sample_app_screenshot.png


