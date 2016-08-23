# DataTemplate，ControlTemplate, ItemsPanelTemplate，HierarchicalDataTemplate
	1. All derive from the abstract FrameworkTemplate class.
	2. DataTemplate is used to create a visualization of a non-visual object, such as a business object.
	3. ControlTemplate supplies a visual representation of a UI control, such as a Button or ListView.
	4. ItemsControl, and all of its subclasses (such as ListBox), create the layout panel that hosts their child elements via an ItemsPanelTemplate.
	5. HierarchicalDataTemplate, which is a data template that has knowledge of how to display a data object’s child objects, such as in a master-detail situation.
## ControlTemplate
说明：用以控件控件的外观，如下代码就是用来设计一个自定义的按钮样式。
```cs
        <Style TargetType="Button">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border BorderThickness="1" BorderBrush="Red" CornerRadius="5">
                            1<!--<ContentControl VerticalAlignment="Center" HorizontalAlignment="Center" Content="{TemplateBinding Content}"
                                            ContentTemplate="{TemplateBinding ContentTemplate}"/>-->
                            2<!--<ContentPresenter VerticalAlignment="Center" HorizontalAlignment="Center" Content="{TemplateBinding Content}" ContentTemplate="{TemplateBinding ContentTemplate}"/>-->
                            3<ContentPresenter VerticalAlignment="Center" HorizontalAlignment="Center"/>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
```
运行结果：  
注意点：  
示例代码中1，2，3片段用那一个都是可以的，相同的结果，其中2，3是等价的，使用ContentPresenter时会隐式的进行TemplateBinding，这的确是经常需要的。其中1最终也是使用了ContentPresenter用来呈现内容，所以不建议使用ContentControl，有点大才小用的赶脚。实际上也是如此，ContentControl中包含ContentPresenter.
## ItemsPanelTemplate  
说明：用以定义集合控件的容器外观，如ListBox,Combox 等等，这里使用一个自定义的listBox用以说明，其默认外观是上下排列，这里修改成横向排列。  
示例代码：
```cs
        <ListBox HorizontalAlignment="Left" Height="100" Margin="49,111,0,0" VerticalAlignment="Top" Width="100">
            <system:String>abc</system:String>
            <system:String>def</system:String>
            <system:String>hij</system:String>
        </ListBox>
        <ListBox HorizontalAlignment="Left" Height="31" Margin="184,111,0,0" VerticalAlignment="Top" Width="100">
            <ListBox.ItemsPanel>
                <ItemsPanelTemplate>
                    <StackPanel Orientation="Horizontal"></StackPanel>
                </ItemsPanelTemplate>
            </ListBox.ItemsPanel>
            <system:String>abc</system:String>
            <system:String>def</system:String>
            <system:String>hij</system:String>
        </ListBox>
```  
运行结果：
## DataTemplate
说明：以命名来看，是用来表现数据外观的，先上一个简单的示例：
```cs
    public class Person
    {
        public string Name { get; set; }
        public string Des { get; set; }
    }

<wpfApplication1:Person x:Key="p1" Name="P1" Des="Girl"/>
       
        <DataTemplate x:Key="dt1" DataType="wpfApplication1:Person">
            <TextBlock>
                    <Run Text="{Binding Name}"/>
                    <Run Text=":"/>
                    <Run Text="{Binding Des}"/>
            </TextBlock>
        </DataTemplate>

<Button Content="{StaticResource p1}" HorizontalAlignment="Left" Margin="204,219,0,0" VerticalAlignment="Top" Width="193" Height="36"/>
        <Button Content="{StaticResource p1}" ContentTemplate="{StaticResource dt1}"  HorizontalAlignment="Left" Margin="204,275,0,0" VerticalAlignment="Top" Width="193" Height="35"/>
```  
运行结果：  
解析：数据模板用来呈现数据，在这个例子中，直接将Button的内容指向一个Person类，若不指定其DataTemplate，则Button的内容只会简单的显示为绑定对象的ToString内容，指定了Person的显示方式，则会按照预期的想法进行呈现。
## HierarchicalDataTemplate  
该数据模板主要应用于比较复杂的树形结构中，HierarchicalDataTemplate从DataTemplate继承而来，其用法也相当的简单，这里给一个简单的示例即可。
```cs
<DockPanel.Resources>
            <HierarchicalDataTemplate DataType     = "{x :Type local :Person}"
                                ItemsSource = "{Binding Path=Persons}">
                <TextBlock Text ="{Binding Path =Name}"/>
            </HierarchicalDataTemplate>

            <HierarchicalDataTemplate DataType     = "{x :Type local :Child}"
                                ItemsSource = "{Binding Path=Persons}">
                <TextBlock Text ="{Binding Path =Des}"/>
            </HierarchicalDataTemplate>
        </DockPanel.Resources>
        <TreeView x :Name="st">
          
        </TreeView>

public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded( object sender, RoutedEventArgs e)
        {
   
            st.ItemsSource = new List< Person>(){ Person.GetRandomPerson(),Person .GetRandomPerson() };
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Des { get; set; }

        public List< Child> Persons { get; set; }

        public static Person GetRandomPerson( int count=0)
        {
            var random = new Random( DateTime.Now.Millisecond);
            var next = random.Next();
            var person = new Person {Name = "name" + next, Des = "des" + next};
            person.Persons = new List< Child>();
            if (count++ < 5)
            {
                for ( int i = 0; i < next % 10; i++)
                {
                    person.Persons.Add( Child.GetRandomPerson(count));
                }
            }
            return person;
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public string Des { get; set; }
        public List< Child> Persons { get; set; }
        public static Child GetRandomPerson( int count = 0)
        {
            var random = new Random( DateTime.Now.Millisecond);
            var next = random.Next();
            var person = new Child { Name = "name" + next, Des = "des" + next };
            person.Persons = new List< Child>();
            if (count++ < 2)
            {
                for ( int i = 0; i < next % 10; i++)
                {
                    person.Persons.Add(GetRandomPerson(count));
                }
            }
            return person;
        }
    }
```  
运行结果：
## StyleSelector and DataTemplateSelector  
考虑到这样一种场景，集合控件需要根据不同的行去设置不同的行样式及内容呈现方式，这就用到了这两个Selector，需要继承这两个类，定义自己需要的效果。同样的，以代码作为示例，并演示相关结果
```cs
class MyStyleSelector : StyleSelector
    {
        public Style StyleContainsO { get; set; }
        public Style StyleOther { get; set; }

        public override Style SelectStyle( object item, DependencyObject container)
        {
            Style re;

            ItemsControl itmesControl = ItemsControl.ItemsControlFromItemContainer(container);

            var index = itmesControl.ItemContainerGenerator.IndexFromContainer(container);

            if (index%3==0)
                re = StyleContainsO;
            else
            {
                re = StyleOther;
            }
            return re;
        }
    }

    class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataContainsO { get ; set ; }
        public DataTemplate DataOther { get ; set ; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item.ToString().Contains( "O"))
                return DataContainsO;
            return DataOther;
         
        }
    }

    <Window.Resources >
        <!-- 数据 -->
        <x: Array xmlns: sys="clr-namespace:System;assembly=mscorlib"
             Type="sys:String"
             x:Key ="array">
            <sys: String>Op</sys :String>
            <sys: String>Kip</sys :String>
            <sys: String>Opp</sys :String>
            <sys: String>Oms</sys :String>
            <sys: String>Kmp</sys :String>
        </x: Array>

        <!-- 自定义3种样式 -->
        <Style x :Key="st1" TargetType="ListBoxItem">
            <Setter Property ="Background" Value="Gray" />
            <Setter Property ="HorizontalContentAlignment" Value="Center" />
        </Style>

        <Style x :Key="st2" TargetType="ListBoxItem">
            <Setter Property ="Background" Value="DarkOliveGreen" />
            <Setter Property ="HorizontalContentAlignment" Value="Center" />
        </Style>

        <DataTemplate x :Key="d1">
            <TextBlock Text ="{Binding Mode =OneWay}"></TextBlock>

        </DataTemplate>

        <DataTemplate x :Key="d2">
            <TextBox Text ="{Binding Mode =OneWay}"></TextBox>
        </DataTemplate>

        <!-- XML命名空间loc是MyStyleSelector的CLR命名空间 -->
        <local: MyStyleSelector
                         x:Key ="mySelector"
                         StyleContainsO="{StaticResource st1}"
                         StyleOther="{StaticResource st2}"/>

        <local: MyDataTemplateSelector
                         x:Key ="myDataSelector"
                         DataContainsO="{StaticResource d1}"
                         DataOther="{StaticResource d2}"/>
    </Window.Resources >
    <ListBox ItemsSource="{StaticResource array}" ItemContainerStyleSelector="{ StaticResource mySelector}" ItemTemplateSelector="{ StaticResource myDataSelector}">
    </ListBox >
```  
运行结果：
     

*DateTemplate缺陷*
http://www.cnblogs.com/nankezhishi/archive/2009/07/08/datatemplate.html
