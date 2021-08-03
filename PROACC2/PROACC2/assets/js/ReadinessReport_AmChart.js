function ConvertToJSON(Data, Name) {
    const mapArrays = (Name, Data) => {
       const res = [];
       for(let i = 0; i < Name.length; i++){
          res.push({
            Name: Name[i],
            count: Data[i]
          });
       };
       return res;
    };
    var result = mapArrays(Name, Data);
    return result;
} 

//Tab Readiness
async function Rediness_SimplificationDonut(Id, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (Data) {
            data_ = [Data.R, Data.RC, Data.RC_NON, Data.R_NON];
            labels = ["Relevant", "Relevance to be Checked", "Relevance to be Checked (Non-strategic)", "Relevant (Non-strategic)"];
            let Data_ = ConvertToJSON(data_, labels)
            Simplication(Data_);
            //SimplificationDonut(data_, labels);

        },
        error: function (e) {

            alert("Error while inserting data");
        }
    });
}
async function Rediness_CustomCode_Bar(Id, URL) {

    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            
            if (Data._List != null) {
                var Output = Data._List;
                var inpName = [];
                var inpVal = [];
                var C = 0;
                for (var i = 0; i < Output.length; i++) {
                    inpName.push(Output[i].Name);
                    inpVal.push(Output[i]._Value);
                }

                labels = inpName;
                data_ = inpVal;
                let Data_CustomCode_Bar = ConvertToJSON(data_, labels);
                CustomCode_Bar(Data_CustomCode_Bar);
                
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Rediness_Activities_Bar1(Id, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            if (Data._List != null) {
                var Output = Data._List;
                var inpName = [];
                var inpVal = [];
                var C = 0;
                for (var i = 0; i < Output.length; i++) {

                    var _Name = Output[i].Name;

                    if (_Name == 'Custom Code Adaption') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'Customizing / Configuration') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'User Training') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else {
                        C = C + Output[i]._Value;
                    }
                }
                inpName.push("Others");
                inpVal.push(C);
                labels = inpName;
                data_ = inpVal;
                let Data_HANA = ConvertToJSON(data_, labels)
                
                HANA(Data_HANA);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Rediness_Activities_Donut(Id, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            if (Data._List != null) {
                var Output = Data._List;
                var inpName = [];
                var inpVal = [];
                var C = 0;

                for (var i = 0; i < Output.length; i++) {

                    inpName.push(Output[i].Name);
                    inpVal.push(Output[i]._Value);
                }
                labels = inpName;
                data_ = inpVal;
                let Data_ = ConvertToJSON(data_, labels);
                Condition(Data_);
                // Activities_Donut(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Rediness_Activities_Bar2(Id, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            if (Data._List != null) {
                var Output = Data._List;
                var inpName = [];
                var inpVal = [];
                var C = 0;
                for (var i = 0; i < Output.length; i++) {

                    var _Name = Output[i].Name;

                    if (_Name == 'During conversion project') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'Before conversion project') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'Before or during conversion project') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else {
                        C = C + Output[i]._Value;
                    }
                }
                inpName.push("Others");
                inpVal.push(C);
                labels = inpName;
                data_ = inpVal;
                let Data_ = ConvertToJSON(data_, labels)
                Convertion(Data_);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Rediness_Fiori_Bar(Id, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            if (Data._List != null) {
                var Output = Data._List;
                var inpName = [];
                var inpVal = [];
                var C = 0;

                for (var i = 0; i < Output.length; i++) {
                    //inpName.push(Output[i].Name);
                    //	inpVal.push(Output[i]._Value);

                    var _Name = Output[i].Name;

                    if (_Name == 'General Ledger Accountant') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'Purchaser') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else if (_Name == 'Cash Management Specialist') {
                        inpName.push(Output[i].Name);
                        inpVal.push(Output[i]._Value);
                    } else {
                        C = C + Output[i]._Value;
                    }
                }
                inpName.push("Others");
                inpVal.push(C);
                labels = inpName;
                data_ = inpVal;
                let Data_ = ConvertToJSON(data_, labels);
            
                //Fiori_Bar(data_, labels);
                FioriApps(Data_);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}

async function Readiness_ECCHanaCount(Id, URL,Table_Count_URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (Data) {

            $("#HanaCount_Summary").empty();
            var summ = '<h6>Summary</h6>';
            summ += '<div><a>Total no. of named users: <b>' + Data.ECC + '</b></a></div>';
            summ += '<div><a>Number of active named users: <b>' + Data.Hana + '</b></a></div>';
            summ += '<div><a>Number of inactive named users: <b>' + (Data.ECC - Data.Hana)+'</b></a></div>';            
            $("#HanaCount_Summary").append(summ);

            data_ = [Data.ECC, Data.Hana];
            labels = ["ECC", "S/4Hana"];
            let Data_ = ConvertToJSON(data_, labels);
            ECCHanaCount(Data_);
        },
        error: function (e) {

            alert("Error while inserting data");
        }
    });

    $.ajax({
        cache: true,
        type: "GET",
        async: false,
        url: Table_Count_URL,
        data: { Instance: Id },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            $("#Id_ECCHanaCount").empty();
            var S_No = 0;
            
            $(Data).each(function (index, value) {
                S_No += 1;
                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="">' + S_No + '</span></td>';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="' + value.User + '">' + value.User + '</span></td>';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="' + value.UserType + '">' + value.UserType + '</span></td>';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="' + value.UserCategory + '">' + value.UserCategory + '</span></td>';
                //table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="">' + value.User_Status + '</span></td>';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="' + value.System + '">' + value.System + '</span></td>';
                table += '<td><span class="" data-placement="top" data-toggle="tooltip" title="' + value.Last_Logon + '">' + value.Last_Logon + '</span></td>';
                table += '</tr > ';

                $('#Id_ECCHanaCount').append(table);
            });
        },
        error: function (e) {
            alert(e.responseText);
        }
    });
}

//Charts
//ECC s4 Hana
function ECCHanaCount(Data) {
    var chart = am4core.create("chartdiv_ECC", am4charts.XYChart);
    chart.data = Data;
    // Add data
    //chart.data = [{
    //    "Name": "ECC",
    //    "count": 5260
    //}, {
    //    "Name": "S/4Hana",
    //    "count": 4170
    //}];

    // Create axes

    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Name";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 30;    
    //categoryAxis.renderer.labels.template.disabled = true;
    categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
        if (target.dataItem && target.dataItem.index & 2 == 2) {
            return dy + 25;
        }
        return dy;
    });

    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.title.text = "Users";
    valueAxis.min = 0;
    valueAxis.max = 7500;

    let label = categoryAxis.renderer.labels.template;
    label.truncate = true;
    label.maxWidth = 120;
    label.tooltipText = "{Name}";
    //label.disabled = true;

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "count";
    series.dataFields.categoryX = "Name";
    //series.name = "count";
    series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
    series.columns.template.fillOpacity = .8;

    //series.calculatePercent = true;
    //series.dataFields.valueYShow = "changePercent";

    var columnTemplate = series.columns.template;
    columnTemplate.strokeWidth = 2;
    columnTemplate.strokeOpacity = 1;
    columnTemplate.stroke = am4core.color("#FFFFFF");

    columnTemplate.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    columnTemplate.adapter.add("stroke", function (stroke, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    chart.cursor = new am4charts.XYCursor();
    chart.cursor.lineX.strokeOpacity = 0;
}
//Simplication
function Simplication(Data) {
    // Themes begin
    // am4core.useTheme(am4themes_dataviz);
    // am4core.useTheme(am4themes_animated);
    // Themes end
    
    // Create chart instance
    var chart = am4core.create("chartdiv_simp", am4charts.PieChart);
    chart.data = Data;
     //Add data
    //  chart.data = [{
    //      "Name": "Relevant(NON-stategic)",
    //      "count": 4
    //  }, {
    //      "Name": "Relevance to be checked(NON-stategic)",
    //      "count": 2
    //  }, {
    //      "Name": "Relevance to be checked",
    //      "count": 14
    //  }, {
    //      "Name": "Relevant",
    //      "count": 38
    //  }];

    // Add and configure Series
    var pieSeries = chart.series.push(new am4charts.PieSeries());
    pieSeries.dataFields.value = "count";
    pieSeries.dataFields.category = "Name";
    pieSeries.innerRadius = am4core.percent(50);
    pieSeries.ticks.template.disabled = true;
    pieSeries.labels.template.disabled = true;

    var rgm = new am4core.RadialGradientModifier();
    rgm.brightnesses.push(-0.8, -0.8, -0.5, 0, - 0.5);
    pieSeries.slices.template.fillModifier = rgm;
    pieSeries.slices.template.strokeModifier = rgm;
    pieSeries.slices.template.strokeOpacity = 0.4;
    pieSeries.slices.template.strokeWidth = 0;

    chart.legend = new am4charts.Legend();
    chart.legend.position = "right";
    chart.legend.maxHeight = 150;
    chart.legend.scrollable = true;

}
//Custom code Analysis
function CustomCode_Bar(Data_CustomCode_Bar) {
    
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    var chart = am4core.create("chartdiv_Customcode", am4charts.XYChart);
    chart.padding(0, 0, 0, 0);
    chart.data = Data_CustomCode_Bar;

    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.dataFields.category = "Name";
    categoryAxis.renderer.minGridDistance = 1;
    categoryAxis.renderer.inversed = true;
    categoryAxis.renderer.grid.template.disabled = true;
    categoryAxis.renderer.labels.template.fontSize = 10;

    let label = categoryAxis.renderer.labels.template;
    label.truncate = true;
    label.maxWidth = 120;
    label.tooltipText = "{Name}";


    chart.cursor = new am4charts.XYCursor();
    chart.cursor.lineX.strokeOpacity = 0;
    //chart.cursor.lineY.strokeOpacity = 0;

    var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
    valueAxis.min = 0;

    var valueyAxis = chart.yAxes.push(new am4charts.ValueAxis());


    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.categoryY = "Name";
    series.dataFields.valueX = "count";
    series.tooltipText = "{valueX.value}"
    series.columns.template.strokeOpacity = 0;
    series.columns.template.column.cornerRadiusBottomRight = 5;
    series.columns.template.column.cornerRadiusTopRight = 5;
    // series.columns.template.tooltipText = "{Name}\n[bold]{code}[/]";
    // series.columns.template.showTooltipOn = "always";

    var labelBullet = series.bullets.push(new am4charts.LabelBullet())
    labelBullet.label.horizontalCenter = "left";
    labelBullet.label.dx = 10;
    labelBullet.label.text = "{values.valueX.workingValue.formatNumber('#.0as')}";
    labelBullet.locationX = 1;

    // as by default columns of the same series are of the same color, we add adapter which takes colors from chart.colors color set
    series.columns.template.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    });

    categoryAxis.sortBySeries = series;
    chart.tooltip.label.maxWidth = 150;
    chart.tooltip.label.wrap = true;


}




//HANA
function HANA(Data_HANA) {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("chartdiv_Activities", am4charts.XYChart3D);
    chart.data = Data_HANA;
    // Add data
    // chart.data = [{
    //     "country": "Custom Code Adaption",
    //     "visits": 38
    // }, {
    //     "country": "Customizing / Configuration",
    //     "visits": 27
    // }, {
    //     "country": "User Training",
    //     "visits": 25
    // }, {
    //     "country": "Others",
    //     "visits": 104
    // }];

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Name";
    //categoryAxis.renderer.labels.template.rotation = 270;
    categoryAxis.renderer.labels.template.hideOversized = false;
    categoryAxis.renderer.minGridDistance = 20;
    // categoryAxis.renderer.labels.template.horizontalCenter = "right";
    // categoryAxis.renderer.labels.template.verticalCenter = "middle";
    //categoryAxis.tooltip.label.rotation = 270;
    //categoryAxis.tooltip.label.horizontalCenter = "right";
    //categoryAxis.tooltip.label.verticalCenter = "middle";

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    //valueAxis.title.text = "Countries";
    //valueAxis.title.fontWeight = "bold";

    let label = categoryAxis.renderer.labels.template;
    label.truncate = true;
    label.maxWidth = 120;
    label.tooltipText = "{Name}";

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries3D());
    series.dataFields.valueY = "count";
    series.dataFields.categoryX = "Name";
    //series.name = "Visits";
    series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
    series.columns.template.fillOpacity = .8;

    var columnTemplate = series.columns.template;
    columnTemplate.strokeWidth = 2;
    columnTemplate.strokeOpacity = 1;
    columnTemplate.stroke = am4core.color("#FFFFFF");

    columnTemplate.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    columnTemplate.adapter.add("stroke", function (stroke, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    chart.cursor = new am4charts.XYCursor();
    chart.cursor.lineX.strokeOpacity = 0;
    //chart.cursor.lineY.strokeOpacity = 0;

}
//Condition
function Condition(Data_) {

    // Themes begin
    // am4core.useTheme(am4themes_dataviz);
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("chartdiv_Condition", am4charts.PieChart3D);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in
    chart.data=Data_;
    // Add data
    //chart.data = [{
    //    "Name": "Optional",
    //    "count": 42
    //}, {
    //    "Name": "Conditional",
    //    "count": 39
    //}, {
    //    "Name": "Mandatory",
    //    "count": 113
    //}];
    chart.innerRadius = am4core.percent(40);
    chart.depth = 20;
    var series = chart.series.push(new am4charts.PieSeries3D());
    series.dataFields.value = "count";
    series.dataFields.depthValue = "count";
    series.dataFields.category = "Name";
    series.slices.template.cornerRadius = 5;
    series.colors.step = 3;

    // Add and configure Series
    // var pieSeries = chart.series.push(new am4charts.PieSeries3D());
    // pieSeries.dataFields.value = "count";
    // pieSeries.dataFields.category = "Name";
    // pieSeries.innerRadius = am4core.percent(50);
    // pieSeries.ticks.template.disabled = true;
    // pieSeries.labels.template.disabled = true;

    var rgm = new am4core.RadialGradientModifier();
    rgm.brightnesses.push(-0.8, -0.8, -0.5, 0, - 0.5);
    pieSeries.slices.template.fillModifier = rgm;
    pieSeries.slices.template.strokeModifier = rgm;
    pieSeries.slices.template.strokeOpacity = 0.4;
    pieSeries.slices.template.strokeWidth = 0;

    chart.legend = new am4charts.Legend();
    chart.legend.position = "right";
    chart.legend.maxHeight = 150;
    chart.legend.scrollable = true;

}

function Convertion(Data_) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("chartdiv_Convertion", am4charts.XYChart3D);
    chart.data = Data_;
    // Add data
    // chart.data = [{
    //     "country": "Custom Code Adaption",
    //     "visits": 38
    // }, {
    //     "country": "Customizing / Configuration",
    //     "visits": 27
    // }, {
    //     "country": "User Training",
    //     "visits": 25
    // }, {
    //     "country": "Others",
    //     "visits": 104
    // }];

    // Create axes
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Name";
    //categoryAxis.renderer.labels.template.rotation = 270;
    categoryAxis.renderer.labels.template.hideOversized = false;
    categoryAxis.renderer.minGridDistance = 20;
    // categoryAxis.renderer.labels.template.horizontalCenter = "right";
    // categoryAxis.renderer.labels.template.verticalCenter = "middle";
    //categoryAxis.tooltip.label.rotation = 270;
    //categoryAxis.tooltip.label.horizontalCenter = "right";
    //categoryAxis.tooltip.label.verticalCenter = "middle";

    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    //valueAxis.title.text = "Countries";
    //valueAxis.title.fontWeight = "bold";

    let label = categoryAxis.renderer.labels.template;
    label.truncate = true;
    label.maxWidth = 120;
    label.tooltipText = "{Name}";

    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries3D());
    series.dataFields.valueY = "count";
    series.dataFields.categoryX = "Name";
    //series.name = "Visits";
    series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
    series.columns.template.fillOpacity = .8;

    var columnTemplate = series.columns.template;
    columnTemplate.strokeWidth = 2;
    columnTemplate.strokeOpacity = 1;
    columnTemplate.stroke = am4core.color("#FFFFFF");

    columnTemplate.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    columnTemplate.adapter.add("stroke", function (stroke, target) {
        return chart.colors.getIndex(target.dataItem.index);
    })

    chart.cursor = new am4charts.XYCursor();
    chart.cursor.lineX.strokeOpacity = 0;
    //chart.cursor.lineY.strokeOpacity = 0;
}

//Convertion
//function Convertion(Data_) {


//    // Themes begin
//    am4core.useTheme(am4themes_animated);
//    // Themes end

//    // Create chart instance
//    var chart = am4core.create("chartdiv_Convertion", am4charts.XYChart3D);
//    chart.paddingBottom = 5;
//    chart.angle = 90;
//    chart.data = Data_;
   
//    // Create axes
//    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
//    categoryAxis.dataFields.category = "Name";
//    categoryAxis.renderer.grid.template.location = 0;
//    categoryAxis.renderer.minGridDistance = 20;
//    categoryAxis.renderer.inside = true;
//    categoryAxis.renderer.grid.template.disabled = true;

//    let labelTemplate = categoryAxis.renderer.labels.template;
//    labelTemplate.rotation = -90;
//    labelTemplate.fontSize = 10;
//    labelTemplate.horizontalCenter = "left";
//    labelTemplate.verticalCenter = "middle";
//    labelTemplate.dy = 10; // moves it a bit down;
//    labelTemplate.inside = false; // this is done to avoid settings which are not suitable when label is rotated

//    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
//    valueAxis.renderer.grid.template.disabled = true;

//    // Create series
//    var series = chart.series.push(new am4charts.ConeSeries());
//    series.dataFields.valueY = "count";
//    series.dataFields.categoryX = "Name";

//    var columnTemplate = series.columns.template;
//    columnTemplate.adapter.add("fill", function (fill, target) {
//        return chart.colors.getIndex(target.dataItem.index);
//    })

//    columnTemplate.adapter.add("stroke", function (stroke, target) {
//        return chart.colors.getIndex(target.dataItem.index);
//    })

//}

//Recommended SAP Fiori Apps

function FioriApps(Data_) {
     // Themes begin
     am4core.useTheme(am4themes_animated);
     // Themes end
 
     var chart = am4core.create("chartdiv_Role_LOB", am4charts.XYChart);
     chart.padding(0, 0, 0, 0);
     chart.data = Data_;
     //  chart.data = [{
     //      "Name": "Non-strategic-function (No equivalent yet)",
     //      "code": 1
     //  }, {
     //      "Name": "Functionality unavailable",
     //      "code": 2
     //  }, {
     //      "Name": "Change of Existing Functionality",
     //      "code": 9
     //  }]
 
     var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
     categoryAxis.renderer.grid.template.location = 0;
     categoryAxis.dataFields.category = "Name";
     categoryAxis.renderer.minGridDistance = 1;
     categoryAxis.renderer.inversed = true;
     categoryAxis.renderer.grid.template.disabled = true;
     categoryAxis.renderer.labels.template.fontSize = 10;
 
     let label = categoryAxis.renderer.labels.template;
     label.truncate = true;
     label.maxWidth = 120;
     label.tooltipText = "{Name}";
 
 
     chart.cursor = new am4charts.XYCursor();
     chart.cursor.lineX.strokeOpacity = 0;
     //chart.cursor.lineY.strokeOpacity = 0;
 
     var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
     valueAxis.min = 0;
 
     var valueyAxis = chart.yAxes.push(new am4charts.ValueAxis());
 
 
     var series = chart.series.push(new am4charts.ColumnSeries());
     series.dataFields.categoryY = "Name";
     series.dataFields.valueX = "count";
     series.tooltipText = "{valueX.value}"
     series.columns.template.strokeOpacity = 0;
     series.columns.template.column.cornerRadiusBottomRight = 5;
     series.columns.template.column.cornerRadiusTopRight = 5;
     // series.columns.template.tooltipText = "{Name}\n[bold]{code}[/]";
     // series.columns.template.showTooltipOn = "always";
 
     var labelBullet = series.bullets.push(new am4charts.LabelBullet())
     labelBullet.label.horizontalCenter = "left";
     labelBullet.label.dx = 10;
     labelBullet.label.text = "{values.valueX.workingValue.formatNumber('#.0as')}";
     labelBullet.locationX = 1;
 
     // as by default columns of the same series are of the same color, we add adapter which takes colors from chart.colors color set
     series.columns.template.adapter.add("fill", function (fill, target) {
         return chart.colors.getIndex(target.dataItem.index);
     });
 
     categoryAxis.sortBySeries = series;
     chart.tooltip.label.maxWidth = 150;
     chart.tooltip.label.wrap = true;
}



//Tab simplification
async function DropDown_Simp(Id, URL) {

    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (data) {

            var insta = $('#IDLob');
            insta.empty();
            insta.append($('<option/>', {
                value: "ALL",
                text: 'ALL'
            }));

            $.each(data._List, function (item, value) {
                var div_data = "<option value=>" + value.Name + "</option>";
                $(div_data).appendTo('#IDLob');
            });
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });

}
async function Table_Simplification(Id, URL) {
    var lob = $('#IDLob :selected').text();

    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id, LOB: lob },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            $("#Id_Simp").empty();
            $(Data).each(function (index, value) {
                var Result = "";
                if (value.SAP_Notes != null) {
                    var d = value.SAP_Notes
                    Result = d.substr(d.length - 6);
                    //Result = (d.Length > 6) ? d.Substring(d.Length - 6, 6) : d;
                }

                //var table = '<tr><td>' + value.S_No +
                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Title + '">' + value.Title + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Category + '">' + value.Category + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Relevance + '">' + value.Relevance + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.LoB_Technology + '">' + value.LoB_Technology + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Business_Area + '">' + value.Business_Area + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Manual_Status + '">' + value.Manual_Status + '</span></td>';
                table += '<td><a href="' + value.SAP_Notes + '" target="_blank">' + Result + '</a></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Relevance_Summary + '">' + value.Relevance_Summary + '</span></td>';
                table += '</tr > ';
                //'<td>' + value.Title +
                //'</td><td>' + value.Category +
                //'</td><td>' + value.Relevance +
                //'</td><td>' + value.LoB_Technology +
                //'</td><td>' + value.Business_Area +
                ////'</td><td>' + value.Consistency_Status +
                //'</td><td>' + value.Manual_Status +
                //'</td><td><a href="' + value.SAP_Notes + '" target="_blank">' + Result + '</a>' +
                //'</td><td>' + value.Relevance_Summary
                //+ '</td></tr>';
                $('#Id_Simp').append(table);
            });
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });

}
async function SimplificationReport_Bar(Id, URL) {
    var lob = $('#IDLob :selected').text();
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id, LOB: lob },
        success: function (Data) {
            var Output = Data._List;
            var inpName = []; var inpVal = []; var backColor = [];
            for (var i = 0; i < Output.length; i++) {
                inpName.push(Output[i].Name);
                inpVal.push(Output[i]._Value);
                backColor.push("#36A2EB");
            }
            //debugger;
            // data_ = [Data.R, Data.RC, Data.RC_NON, Data.R_NON];
            //labels = ["Relevant", "Relevance to be Checked", "Relevance to be Checked (Non-strategic)", "Relevant (Non-strategic)"];
            labels = inpName;
            data_ = inpVal;
            let Data_ = ConvertToJSON(data_, labels);
            SimplificationReport_Bar_Chart(Data_);
            //SimplificationReport_Bar_Chart(data_, labels, backColor);

        },
        error: function () {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
//Chart
function SimplificationReport_Bar_Chart(Data_) {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end
    
    // Create chart instance
    var chart = am4core.create("Chart_Relevant", am4charts.XYChart);
    chart.scrollbarX = new am4core.Scrollbar();
    
    chart.data = Data_;
    

// Create axes
var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
categoryAxis.dataFields.category = "Name";
categoryAxis.renderer.grid.template.location = 0;
categoryAxis.renderer.minGridDistance = 0;
categoryAxis.renderer.labels.template.horizontalCenter = "right";
categoryAxis.renderer.labels.template.verticalCenter = "middle";
categoryAxis.renderer.labels.template.rotation = 270;
    
//categoryAxis.tooltip.disabled = true;
   // categoryAxis.renderer.minHeight = 110;
    categoryAxis.renderer.labels.template.disabled = true;

let labelTemplate = categoryAxis.renderer.labels.template;
    labelTemplate.rotation = -90;
   // labelTemplate.paddingBottom = 10;
    labelTemplate.horizontalCenter = "left";
    labelTemplate.verticalCenter = "middle";
    labelTemplate.dy =0; // moves it a bit down;
    labelTemplate.inside = true; // this is done to avoid settings which are not suitable when label is rotated
    

var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
valueAxis.renderer.minWidth = 0;


// Create series
var series = chart.series.push(new am4charts.ColumnSeries());
series.sequencedInterpolation = true;
series.dataFields.valueY = "count";
series.dataFields.categoryX = "Name";
series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
    series.columns.template.strokeWidth = 0;


series.tooltip.pointerOrientation = "vertical";

series.columns.template.column.cornerRadiusTopLeft = 10;
series.columns.template.column.cornerRadiusTopRight = 10;
series.columns.template.column.fillOpacity = 0.8;

// on hover, make corner radiuses bigger
var hoverState = series.columns.template.column.states.create("hover");
hoverState.properties.cornerRadiusTopLeft = 0;
hoverState.properties.cornerRadiusTopRight = 0;
hoverState.properties.fillOpacity = 1;

series.columns.template.adapter.add("fill", function(fill, target) {
  return chart.colors.getIndex(target.dataItem.index);
});

// Cursor
    chart.cursor = new am4charts.XYCursor();
    //chart.tooltip.label.wrap = true;
    
    }



//Tab Activities
async function DropDown_Ass(Id, URL) {
    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (data) {

            var insta = $('#ID_Phase,#ID_Condition');
            insta.empty();
            insta.append($('<option/>', {
                value: "ALL",
                text: 'ALL'
            }));
            $.each(data.Item1, function (item, value) {
                var div_data = "<option value=>" + value.Name + "</option>";
                $(div_data).appendTo('#ID_Phase');
            });
            $.each(data.Item2, function (item, value) {
                var div_data = "<option value=>" + value.Name + "</option>";
                $(div_data).appendTo('#ID_Condition');
            });
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });

}
async function ActivitiesTable(ID, URL) {

    var Phase = $('#ID_Phase :selected').text();
    var condition = $('#ID_Condition :selected').text();
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: ID, Phase: Phase, condition: condition },
        success: function (Data) {
            $("#Id_Activities").empty();
            $(Data).each(function (index, value) {
                var Result = "";
                if (value.SAP_Notes != null) {
                    var d = value.SAP_Notes
                    Result = d.substr(d.length - 6);
                    //Result = (d.Length > 6) ? d.Substring(d.Length - 6, 6) : d;
                }

                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Related_Simplification_Items + '">' + value.Related_Simplification_Items + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Activities + '">' + value.Activities + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Phase + '">' + value.Phase + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Condition + '">' + value.Condition + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Additional_Information + '">' + value.Additional_Information + '</span></td>';
                table += '</tr > ';

                //var table = '<tr><td>' + value.S_No +
                //    '</td><td>' + value.Related_Simplification_Items +
                //    '</td><td>' + value.Activities +
                //    '</td><td>' + value.Phase +
                //    '</td><td>' + value.Condition+
                //    '</td><td>' + value.Additional_Information
                //    + '</td></tr>';
                $('#Id_Activities').append(table);
            });
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Activities_Report_Bar(ID, URL) {
    var Phase = $('#ID_Phase :selected').text();
    var condition = $('#ID_Condition :selected').text();

    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: ID, Phase: Phase, condition: condition },
        success: function (Data) {

            var Output = Data._List;
            var inpName = [];
            var inpVal = [];
            var backColor = [];
            var C = 0;
            for (var i = 0; i < Output.length; i++) {

                inpName.push(Output[i].Name);
                inpVal.push(Output[i]._Value);
                backColor.push("#36A2EB");
            }
            labels = inpName;
            data_ = inpVal;
            let Data_ = ConvertToJSON(data_, labels);
            ActivitiesBar_Chart(Data_);
            //ActivitiesBar_Chart(data_, labels, backColor);
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
//Chart
function ActivitiesBar_Chart(Data_) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("Chart_Activities", am4charts.XYChart);
    chart.scrollbarX = new am4core.Scrollbar();

    chart.data = Data_;


    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Name";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 0;
    categoryAxis.renderer.labels.template.horizontalCenter = "right";
    categoryAxis.renderer.labels.template.verticalCenter = "middle";
    categoryAxis.renderer.labels.template.rotation = 270;

    //categoryAxis.tooltip.disabled = true;
    // categoryAxis.renderer.minHeight = 110;
    categoryAxis.renderer.labels.template.disabled = true;

    let labelTemplate = categoryAxis.renderer.labels.template;
    labelTemplate.rotation = -90;
    // labelTemplate.paddingBottom = 10;
    labelTemplate.horizontalCenter = "left";
    labelTemplate.verticalCenter = "middle";
    labelTemplate.dy = 0; // moves it a bit down;
    labelTemplate.inside = true; // this is done to avoid settings which are not suitable when label is rotated


    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minWidth = 0;


    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.sequencedInterpolation = true;
    series.dataFields.valueY = "count";
    series.dataFields.categoryX = "Name";
    series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
    series.columns.template.strokeWidth = 0;


    series.tooltip.pointerOrientation = "vertical";

    series.columns.template.column.cornerRadiusTopLeft = 10;
    series.columns.template.column.cornerRadiusTopRight = 10;
    series.columns.template.column.fillOpacity = 0.8;

    // on hover, make corner radiuses bigger
    var hoverState = series.columns.template.column.states.create("hover");
    hoverState.properties.cornerRadiusTopLeft = 0;
    hoverState.properties.cornerRadiusTopRight = 0;
    hoverState.properties.fillOpacity = 1;

    series.columns.template.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    });

    // Cursor
    chart.cursor = new am4charts.XYCursor();
    //chart.tooltip.label.wrap = true;

}

//Tab Landscape


async function SAPDirect(ID, url) {
    
    var val = $("#Drp_Sys_ScanReport").val();
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: url,
        data: { Ins: ID, val: val },
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {

            $("#Id_SAPDirect").empty();
            $("#Id_SAPDirect_head").empty();
            
            var thead = '<tr>';
            thead += '<td scope="col" width=10%>S.No</td>';
            if (val == 1) {                
                thead += '<td scope="col" width=20%>Parameter</td>';
                thead += '<td scope="col" width=35%>Value</td>';
            } else if (val == 2) {
                thead += '<td scope="col" width=10%>Component</td>';
                thead += '<td scope="col" width=10%>Release</td>';
                thead += '<td scope="col" width=10%>SP Level</td>';
                thead += '<td scope="col" width=10%>Comp.Type</td>';
                thead += '<td scope="col" width=25%>Description</td>';
            } else if (val == 3) {
                thead += '<td scope="col" width=20%>Product</td>';
                thead += '<td scope="col" width=15%>Release</td>';
                thead += '<td scope="col" width=15%>SP Stack</td>';
                thead += '<td scope="col" width=15%>Vendor</td>';
                thead += '<td scope="col" width=25%>Description</td>';
            } else if (val == 4) {
                thead += '<td scope="col" width=10%>Group</td>';
                thead += '<td scope="col" width=20%>BF Name</td>';
                thead += '<td scope="col" width=45%>BF Description</td>';
                thead += '<td scope="col" width=20%>Dependency</td>';
                thead += '<td scope="col" width=20%>SW Component</td>';
                thead += '<td scope="col" width=15%>SW Release</td>';
            }            
            thead +='</tr >';
            $("#Id_SAPDirect_head").append(thead);
            
            var i = 0;
            $(Data).each(function (index, value) {
                if (index > 0) {
                    i += 1;
                    if (val == 1)
                    {
                        $('.classcheck').addClass('col-lg-4');
                        $('.classcheck1').removeClass('col-lg-12').addClass('col-lg-8');
                        $("#sys_details").empty();
                        var sID = $("#ProjInstance option:selected").text();
                        var summ = '<h6>Summary</h6>';
                        summ += '<div><a>SID: <b>' + sID + '</b></a></div>';
                        summ += '<div><a>SAP ECC Source System Version: <b>SAP ECC EhP7</b></a></div>';
                        summ += '<div><a> NetWeaver Version and SP: <b>740 SPS 16</b></a></div>';                                                    
                        summ += '<div><a>Type: <b>Unicode</b></a></div>';
                        summ += '<div><a>Database: <b>MSSQL</b></a></div>';
                        summ += '<div><a>OS: <b>Windows Server 2012</b></a></div>';
                        //summ += '<div><a>No. of CPUs: <b>16</b></a></div>';
                        //summ += '<div><a>Database Size: <b>512 GB (Used Space:400 GB  Free Space:112 GB)</b></a></div>';
                        //summ += '<div><a>Clients: <b>000, 001, 066, 100</b></a></div>';
                        $("#sys_details").append(summ);
                    }
                    var table = '<tr class="tr_Effect" >';
                    table += '<td><span class="" data-toggle="tooltip" title="">' + i + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD1 + '">' + value.FIELD1 + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD2 + '">' + value.FIELD2 + '</span></td>';
                    
                    if (val == 2 || val == 3 || val == 4) {
                        $('.classcheck').empty();
                        $('.classcheck').removeClass('col-lg-4');
                        $('.classcheck1').removeClass('col-lg-8').addClass('col-lg-12');

                        table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD3 + '">' + value.FIELD3 + '</span></td>';
                        table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD4 + '">' + value.FIELD4 + '</span></td>';
                        table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD5 + '">' + value.FIELD5 + '</span></td>';
                        if (val == 4) {
                            table += '<td><span class="" data-toggle="tooltip" title="' + value.FIELD6 + '">' + value.FIELD6 + '</span></td>';
                        }
                    }
                    table += '</tr > ';
                }
                
                $('#Id_SAPDirect').append(table);
            });
        },
        error: function () {
            //alert("Error while inserting data");
            var msg = "SAP System Server Down";
            Notiflix.Report.Info("ProAcc Info !", msg,
                "okay",
                function () {
                    $("#SYSTEM_DETAILS").hide();
                    $("#Id_SAPDirect").empty();
                    $("#Id_SAPDirect_head").empty();
                    $("#Drp_Sys_ScanReport").val(0);
                }
            );

        }
    }).done(function (response) {
        //chart.data = response;

    });
}

async function summery_sys_report(val, summery_URL, sys_LandScape) {
    
    if (val == 1) {
        $.ajax({
            cache: false,
            type: "GET",
            url: summery_URL,
            data: { Instance: $("#ProjInstance").val(), sys_LandScape: sys_LandScape },
            dataType: "json",
            success: function (Data) {
                $('.classcheck').addClass('col-lg-4');
                $('.classcheck1').removeClass('col-lg-12').addClass('col-lg-8');
                $("#sys_details").empty();
                var sID = $("#ProjInstance option:selected").text();
                var summ = '<h6>Summary</h6>';
                summ += '<div><a>SID: <b>' + sID + '</b></a></div>';
                summ += '<div><a>SAP ECC Source System Version: <b>' + Data.SAPECC+'</b></a></div>';
                summ += '<div><a> NetWeaver Version and SP: <b>' + Data.NetWeaver + '</b></a></div>';
                if (Data.TypeVersion == 'YES') {
                    summ += '<div><a>Type: <b>Unicode</b></a></div>';
                }
                else {
                    summ += '<div><a>Type: <b>Non Unicode</b></a></div>';
                }
                
                summ += '<div><a>Database: <b>' + Data.Database +'</b></a></div>';
                summ += '<div><a>OS: <b>' + Data.OS +'</b></a></div>';
                $("#sys_details").append(summ);
            },
            error: function () {
                alert("Error in summery_sys_report");
            }
        });
    }

}

async function Table_SAPDirect_DB(Id, sys_LandScape, URL,summery_URL) {
    var val = $("#Drp_Sys_ScanReport").val();
    
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id, val: val, sys_LandScape: sys_LandScape},
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            $("#Id_SAPDirect").empty();

            $("#Id_SAPDirect_head").empty();

            var thead = '<tr>';
            thead += '<td scope="col" width=10%>S.No</td>';
            if (val == 1) {                
                thead += '<td scope="col" width=20%>Parameter</td>';
                thead += '<td scope="col" width=35%>Value</td>';
            } else if (val == 2) {
                $('.classcheck').empty();
                $('.classcheck').removeClass('col-lg-4');
                $('.classcheck1').removeClass('col-lg-8').addClass('col-lg-12');

                thead += '<td scope="col" width=10%>Component</td>';
                thead += '<td scope="col" width=10%>Release</td>';
                thead += '<td scope="col" width=10%>SP Level</td>';
                thead += '<td scope="col" width=10%>Comp.Type</td>';
                thead += '<td scope="col" width=25%>Description</td>';
            } else if (val == 3) {
                $('.classcheck').empty();
                $('.classcheck').removeClass('col-lg-4');
                $('.classcheck1').removeClass('col-lg-8').addClass('col-lg-12');

                thead += '<td scope="col" width=20%>Product</td>';
                thead += '<td scope="col" width=15%>Release</td>';
                thead += '<td scope="col" width=15%>SP Stack</td>';
                thead += '<td scope="col" width=15%>Vendor</td>';
                thead += '<td scope="col" width=25%>Description</td>';
            } else if (val == 4) {
                $('.classcheck').empty();
                $('.classcheck').removeClass('col-lg-4');
                $('.classcheck1').removeClass('col-lg-8').addClass('col-lg-12');

                thead += '<td scope="col" width=10%>Group</td>';
                thead += '<td scope="col" width=20%>BF Name</td>';
                thead += '<td scope="col" width=45%>BF Description</td>';
                thead += '<td scope="col" width=20%>Dependency</td>';
                thead += '<td scope="col" width=20%>SW Component</td>';
                thead += '<td scope="col" width=15%>SW Release</td>';
            }
            thead += '</tr >';
            $("#Id_SAPDirect_head").append(thead);

            if (val == 1) {
                //var URL = '@Url.Action("summery_sys_report")';
                summery_sys_report(val, summery_URL, sys_LandScape);

                //$('.classcheck').addClass('col-lg-4');
                //$('.classcheck1').removeClass('col-lg-12').addClass('col-lg-8');
                //$("#sys_details").empty();
                //var sID = $("#ProjInstance option:selected").text();
                //var summ = '<h6>Summary</h6>';
                //summ += '<div><a>SID: <b>' + sID + '</b></a></div>';
                //summ += '<div><a>SAP ECC Source System Version: <b>SAP ECC EhP7</b></a></div>';
                //summ += '<div><a> NetWeaver Version and SP: <b>740 SPS 16</b></a></div>';
                //summ += '<div><a>Type: <b>Unicode</b></a></div>';
                //summ += '<div><a>Database: <b>MSSQL</b></a></div>';
                //summ += '<div><a>OS: <b>Windows Server 2012</b></a></div>';
                //$("#sys_details").append(summ);
            }

            $(Data).each(function (index, value) {
                var Result = "";

                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';                
                if (val == 1) {                    
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Parameter + '">' + value.Parameter + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Value + '">' + value.Value + '</span></td>';
                }
                else if (val == 2) {
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Component + '">' + value.Component + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Release + '">' + value.Release + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.SP_Level + '">' + value.SP_Level + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Comp_Type + '">' + value.Comp_Type + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Description + '">' + value.Description + '</span></td>';
                }
                else if (val == 3) {
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Product + '">' + value.Product + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Release + '">' + value.Release + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.SP_Stack + '">' + value.SP_Stack + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Vendor + '">' + value.Vendor + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Description + '">' + value.Description + '</span></td>';
                }
                else if (val == 4) {
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Group + '">' + value.Group + '</span></td>';                    
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.BF_Name + '">' + value.BF_Name + '</span></td>';                    
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Description + '">' + value.Description + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Dependency + '">' + value.Dependency + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Component + '">' + value.Component + '</span></td>';
                    table += '<td><span class="" data-toggle="tooltip" title="' + value.Release + '">' + value.Release + '</span></td>';
                }
                table += '</tr > ';

                $('#Id_SAPDirect').append(table);
            });
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}

async function HanaDataBaseTables(ID, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: ID },
        success: function (Data) {
            $("#S4HanaDatabase_Tables").empty();
            $(Data).each(function (index, value) {
                var Result = "";
               
                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Name + '">' + value.Name + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.StoreType + '">' + value.StoreType + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.DataSize + '">' + value.DataSize + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.No_of_Records + '">' + value.No_of_Records + '</span></td>';
                table += '</tr > ';
               
                $('#S4HanaDatabase_Tables').append(table);
            });
        },
        error: function (a) {
            alert(a);
            alert("Error while getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}

//Tab Custom Code Analysis
async function CustomCodeTable(ID, URL) {
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: ID },
        success: function (Data) {
            $("#Id_Custom_Code").empty();
            $(Data).each(function (index, value) {
                var Result = "";
                if (value.Custom_Code_Note != null) {
                    var d = value.Custom_Code_Note
                    Result = d.substr(d.length - 6);
                    //Result = (d.Length > 6) ? d.Substring(d.Length - 6, 6) : d;
                }
                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Custom_Code_Topic + '">' + value.Custom_Code_Topic + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Status + '">' + value.Status + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Application_Component + '">' + value.Application_Component + '</span></td>';
                table += '<td><a href="' + value.Custom_Code_Note + '" target="_blank">' + Result + '</a></td>';
                //table += '<td><span class="" data-toggle="tooltip" title="' + value.Custom_Code_Note + '">' + value.Custom_Code_Note + '</span></td>';
                table += '</tr > ';

                //var table = '<tr><td>' + value.S_No +
                //    '</td><td>' + value.Custom_Code_Topic +
                //    '</td><td>' + value.Status +
                //    '</td><td>' + value.Application_Component +
                //    '</td><td>' + value.Custom_Code_Note
                //    + '</td></tr>';
                $('#Id_Custom_Code').append(table);
            });
        },
        error: function (a) {
            alert(a);
            alert("Error while getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function CustomCode_Bar_(ID, URL) {

    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: ID },
        success: function (Data) {
            var Output = Data._List;
            var inpName = [];
            var inpVal = [];
            var backColor = [];
            var C = 0;
            for (var i = 0; i < Output.length; i++) {

                inpName.push(Output[i].Name);
                inpVal.push(Output[i]._Value);
                backColor.push("#36A2EB");
            }
            //inpName.push("Others");
            //inpVal.push(C);
            labels = inpName;
            data_ = inpVal;
            let Data_ = ConvertToJSON(data_, labels);
            CustomBarChart(Data_);
            //CustomBarChart(data_, labels, backColor);
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
function CustomBarChart(Data_) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("Chart_Custom_Code", am4charts.XYChart);
    chart.scrollbarX = new am4core.Scrollbar();

    chart.data = Data_;


    // Create axes
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Name";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 0;
    categoryAxis.renderer.labels.template.horizontalCenter = "right";
    categoryAxis.renderer.labels.template.verticalCenter = "middle";
    categoryAxis.renderer.labels.template.rotation = 270;

    //categoryAxis.tooltip.disabled = true;
    // categoryAxis.renderer.minHeight = 110;
    categoryAxis.renderer.labels.template.disabled = true;

    let labelTemplate = categoryAxis.renderer.labels.template;
    labelTemplate.rotation = -90;
    // labelTemplate.paddingBottom = 10;
    labelTemplate.horizontalCenter = "left";
    labelTemplate.verticalCenter = "middle";
    labelTemplate.dy = 0; // moves it a bit down;
    labelTemplate.inside = true; // this is done to avoid settings which are not suitable when label is rotated


    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.minWidth = 0;


    // Create series
    var series = chart.series.push(new am4charts.ColumnSeries());
    series.sequencedInterpolation = true;
    series.dataFields.valueY = "count";
    series.dataFields.categoryX = "Name";
    series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
    series.columns.template.strokeWidth = 0;


    series.tooltip.pointerOrientation = "vertical";

    series.columns.template.column.cornerRadiusTopLeft = 10;
    series.columns.template.column.cornerRadiusTopRight = 10;
    series.columns.template.column.fillOpacity = 0.8;

    // on hover, make corner radiuses bigger
    var hoverState = series.columns.template.column.states.create("hover");
    hoverState.properties.cornerRadiusTopLeft = 0;
    hoverState.properties.cornerRadiusTopRight = 0;
    hoverState.properties.fillOpacity = 1;

    series.columns.template.adapter.add("fill", function (fill, target) {
        return chart.colors.getIndex(target.dataItem.index);
    });

    // Cursor
    chart.cursor = new am4charts.XYCursor();
    //chart.tooltip.label.wrap = true;

}


//Tab Fiori
async function DropDown_Fiori(Id, URL) {
    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (data) {

            var insta = $('#IDRoles');
            insta.empty();
            insta.append($('<option/>', {
                value: "ALL",
                text: 'ALL'
            }));

            $.each(data._List, function (item, value) {
                var div_data = "<option value=>" + value.Name + "</option>";
                $(div_data).appendTo('#IDRoles');
            });
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function FioriTable(Id, URL) {
    var Role = $('#IDRoles :selected').text();
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id, Role: Role },
        success: function (Data) {
            $("#Id_Fiori").empty();
            $(Data).each(function (index, value) {
                //var Result = "";
                //if (value.SAP_Notes != null) {
                //    var d = value.SAP_Notes
                //     Result = d.substr(d.length - 6);
                //    //Result = (d.Length > 6) ? d.Substring(d.Length - 6, 6) : d;
                //}
                var table = '<tr class="tr_Effect" >';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.S_No + '">' + value.S_No + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Role + '">' + value.Role + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Name + '">' + value.Name + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Application_Area + '">' + value.Application_Area + '</span></td>';
                table += '<td><span class="" data-toggle="tooltip" title="' + value.Description + '">' + value.Description + '</span></td>';
                table += '</tr > ';
                //var table = '<tr><td>' + value.S_No +
                //    '</td><td>' + value.Role +
                //    '</td><td>' + value.Name +
                //    '</td><td>' + value.Application_Area +
                //    '</td><td>' + value.Description
                //    + '</td></tr>';
                $('#Id_Fiori').append(table);
            });

        },
        error: function () {
            //$("#load").css("display", "hide");
            alert("Error while inserting data");

        }
    }).done(function (response) {
        //chart.data = response;
        //$("#load").css("display", "hide");
    });
    //$("#load").css("display", "hide");
}
async function FioriReport_Bar(Id, URL) {
    var Role = $('#IDRoles :selected').text();
    $.ajax({
        cache: false,
        type: "GET",
        async: false,
        url: URL,
        data: { Instance: Id, Role: Role },
        contentType: "application/json; charset=utf-8",
        success: function (Data) {
            var Output = Data._List;

            var inpName = []; var inpVal = []; var backColor = [];
            for (var i = 0; i < Output.length; i++) {
                inpName.push(Output[i].Name);
                inpVal.push(Output[i]._Value);
                backColor.push("#36A2EB");
            }
            labels = inpName;
            data_ = inpVal;
            let Data_ = ConvertToJSON(data_, labels);
            Fiori_Chart(Data_)
            //renderChart(data_, labels, backColor);
            //Fiori_Chart(data_, labels, backColor);

        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {

        //chart.data = response;

    });
}
//Chart
function Fiori_Chart(Data_) {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    var chart = am4core.create("Chart_FioriApp", am4charts.XYChart);

    //var data = [];
    //var value = 120;

    //var names = ["Raina",
    //    "Demarcus",
    //    "Carlo",
    //    "Jacinda",
    //    "Richie",
    //    "Antony",
    //    "Amada",
    //    "Idalia",
    //    "Janella",
    //    "Marla",
    //    "Curtis",
    //    "Shellie",
    //    "Meggan",
    //    "Nathanael",
    //    "Jannette",
    //    "Tyrell",
    //    "Sheena",
    //    "Maranda",
    //    "Briana",
    //    "Rosa",
    //    "Rosanne",
    //    "Herman",
    //    "Wayne",
    //    "Shamika",
    //    "Suk",
    //    "Clair",
    //    "Olivia",
    //    "Hans",
    //    "Glennie",
    //];

    //for (var i = 0; i < names.length; i++) {
    //    value += Math.round((Math.random() < 0.5 ? 1 : -1) * Math.random() * 5);
    //    data.push({ category: names[i], value: value });
    //}

    chart.data = Data_;
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.dataFields.category = "Name";
    categoryAxis.renderer.minGridDistance = 15;
    categoryAxis.renderer.grid.template.location = 0.5;
    categoryAxis.renderer.grid.template.strokeDasharray = "1,3";
    //categoryAxis.renderer.labels.template.rotation = -90;
   // categoryAxis.renderer.labels.template.horizontalCenter = "left";
    //categoryAxis.renderer.labels.template.location = 0.5;

    //categoryAxis.renderer.grid.template.disabled = true;
    categoryAxis.renderer.labels.template.disabled = true;

    //let labelTemplate = categoryAxis.renderer.labels.template;
    //labelTemplate.rotation = -90;
    //labelTemplate.horizontalCenter = "left";
    //labelTemplate.verticalCenter = "middle";

   

    //labelTemplate.dy = 20; // moves it a bit down;
    //labelTemplate.inside = true; // this is done to avoid settings which are not suitable when label is rotated

    //let labelTemplateX = categorxAxis.renderer.label.template;
    //labelTemplateX.rotation = -90;
    //labelTemplateX.horizontalCenter = "left";
    //labelTemplateX.verticalCenter = "middle";

    categoryAxis.renderer.labels.template.adapter.add("dx", function (dx, target) {
        return -target.maxRight / 2;
    })
    //var valuexAxis = chart.xAxes.push(new am4charts.ValueAxis());
    //valuexAxis.min = 0;
    

    var valueyAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueyAxis.tooltip.disabled = true;
    valueyAxis.renderer.ticks.template.disabled = true;
    valueyAxis.renderer.axisFills.template.disabled = true;
    

    var series = chart.series.push(new am4charts.ColumnSeries());
    series.dataFields.categoryX = "Name";
    series.dataFields.valueY = "count";
    //series.tooltipText = "{valueY.value}";
    series.tooltipText = "[{categoryX}: bold]{valueY}[/]";
    series.sequencedInterpolation = true;
    series.fillOpacity = 0;
    series.strokeOpacity = 1;
    series.strokeDashArray = "1,3";
    series.columns.template.width = 0.01;
    series.tooltip.pointerOrientation = "horizontal";

    var bullet = series.bullets.create(am4charts.CircleBullet);

    chart.cursor = new am4charts.XYCursor();

    chart.scrollbarX = new am4core.Scrollbar();
    chart.scrollbarY = new am4core.Scrollbar();
   // chart.tooltip.label.wrap = true;

    
}


async function Load_PlannedVsCompletion(ID, URL) {
    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: ID },
        success: function (_data) {
            PlannedVsCompletion(_data);
           // var Planed=[];
           // var Actual=[];
           // $.each(data, function (index, value) {
           //     console.log(value);
           //     Planed.push( new Date(value.Planed__En_Date));
           //     Actual.push(new Date (value.Actual_En_Date));
           // });

           // debugger;
           //// var obj = Object.value(data);
           // var dateToday = new Date(year, month - 1, day);
           // var dateEndPlacement = new Date(2014, 5, 22);

           // weeksBetween(dateToday, dateEndPlacement);


            //var insta = $('#IDRoles');
            //insta.empty();
            //insta.append($('<option/>', {
            //    value: "ALL",
            //    text: 'ALL'
            //}));

            //$.each(data._List, function (item, value) {
            //    var div_data = "<option value=>" + value.Name + "</option>";
            //    $(div_data).appendTo('#IDRoles');
            //});
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
   
}
function weeksBetween(d1, d2) {
    return Math.round((d2 - d1) / (7 * 24 * 60 * 60 * 1000));
}

//Project View 
function PlannedVsCompletion1() {

    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    // Create chart instance
    var chart = am4core.create("ChartDiv_PlannedVsComp", am4charts.XYChart);

    chart.data = [{
        "year": "Name of the task",
        "italy": new Date(2019,3,20),
        "germany": new Date(2019, 4, 25)
    }, {
            "year": "gdfgdfdfgdgd",
            "italy": new Date(2019, 5, 20),
            "germany": new Date(2019, 6, 20)
        }, {
            "year": "dfgdgfd",
            "italy": new Date(2019, 7, 20),
            "germany": new Date(2019, 8, 20)
        }
    ];



    //// Add data
    //chart.data = [{
    //    "year": "1930",
    //    "italy": 1,
    //    "germany": 5,
    //    "uk": 3
    //}, {
    //    "year": "1934",
    //    "italy": 1,
    //    "germany": 2,
    //    "uk": 6
    //}, {
    //    "year": "1938",
    //    "italy": 2,
    //    "germany": 3,
    //    "uk": 1
    //}, {
    //    "year": "1950",
    //    "italy": 3,
    //    "germany": 4,
    //    "uk": 1
    //}, {
    //    "year": "1954",
    //    "italy": 5,
    //    "germany": 1,
    //    "uk": 2
    //}, {
    //    "year": "1958",
    //    "italy": 3,
    //    "germany": 2,
    //    "uk": 1
    //}, {
    //    "year": "1962",
    //    "italy": 1,
    //    "germany": 2,
    //    "uk": 3
    //}, {
    //    "year": "1966",
    //    "italy": 2,
    //    "germany": 1,
    //    "uk": 5
    //}, {
    //    "year": "1970",
    //    "italy": 3,
    //    "germany": 5,
    //    "uk": 2
    //}, {
    //    "year": "1974",
    //    "italy": 4,
    //    "germany": 3,
    //    "uk": 6
    //}, {
    //    "year": "1978",
    //    "italy": 1,
    //    "germany": 2,
    //    "uk": 4
    //}];

    // Create category axis
    var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "year";
    categoryAxis.renderer.opposite = true;

    // Create value axis
    var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.inversed = true;
    valueAxis.title.text = "Place taken";
    valueAxis.renderer.minLabelPosition = 0.01;

    // Create series
    var series1 = chart.series.push(new am4charts.LineSeries());
    series1.dataFields.valueY = "italy";
    series1.dataFields.categoryX = "year";
    series1.name = "Italy";
    series1.bullets.push(new am4charts.CircleBullet());
    series1.tooltipText = "Place taken by {name} in {categoryX}: {valueY}";
    series1.legendSettings.valueText = "{valueY}";
    series1.visible = false;

    var series2 = chart.series.push(new am4charts.LineSeries());
    series2.dataFields.valueY = "germany";
    series2.dataFields.categoryX = "year";
    series2.name = 'Germany';
    series2.bullets.push(new am4charts.CircleBullet());
    series2.tooltipText = "Place taken by {name} in {categoryX}: {valueY}";
    series2.legendSettings.valueText = "{valueY}";

    //var series3 = chart.series.push(new am4charts.LineSeries());
    //series3.dataFields.valueY = "uk";
    //series3.dataFields.categoryX = "year";
    //series3.name = 'United Kingdom';
    //series3.bullets.push(new am4charts.CircleBullet());
    //series3.tooltipText = "Place taken by {name} in {categoryX}: {valueY}";
    //series3.legendSettings.valueText = "{valueY}";

    // Add chart cursor
    chart.cursor = new am4charts.XYCursor();
    chart.cursor.behavior = "zoomY";


    let hs1 = series1.segments.template.states.create("hover")
    hs1.properties.strokeWidth = 5;
    series1.segments.template.strokeWidth = 1;

    let hs2 = series2.segments.template.states.create("hover")
    hs2.properties.strokeWidth = 5;
    series2.segments.template.strokeWidth = 1;

    //let hs3 = series3.segments.template.states.create("hover")
    //hs3.properties.strokeWidth = 5;
    //series3.segments.template.strokeWidth = 1;

    // Add legend
    chart.legend = new am4charts.Legend();
    chart.legend.itemContainers.template.events.on("over", function (event) {
        var segments = event.target.dataItem.dataContext.segments;
        segments.each(function (segment) {
            segment.isHover = true;
        })
    })

    chart.legend.itemContainers.template.events.on("out", function (event) {
        var segments = event.target.dataItem.dataContext.segments;
        segments.each(function (segment) {
            segment.isHover = false;
        })
    })

}
function PlannedVsCompletion(_data ) {
    //debugger;
    
    
    //var Jason = {};
    //var Planed = []
    //Jason.Planed = Planed;
    //console.log(Jason);

    //var Plan = "Planed";
    //var Act = "Actual";
    //var Data_ = {
    //    "Planed": Plan,
    //    "Actual": Act
    //}

    //var Data_ = {
    //    "category": "Planed",
    //    "start": "2016-01-10",
    //    "end": "2016-01-13",
    //    "color": colorSet.getIndex(0),
    //    "task": "Gathering requirements"
    //}

    //Jason.Planed.push(Data_);
    //console.log(Jason);
    //debugger;
    am4core.useTheme(am4themes_animated);
    // Themes end

    var chart = am4core.create("ChartDiv_PlannedVsComp", am4plugins_timeline.SerpentineChart);
    //chart.curveContainer.padding(20, 20, 20, 20);
    chart.levelCount = 8;
    chart.orientation = "horizontal";
    chart.fontSize = 11;

    var Jason = {};
    var Data_ = []
    Jason.Data_ = Data_;
    var colorSet = new am4core.ColorSet();
    colorSet.saturation = 0.6;

    $.each(_data, function (key, value) {
        var Data_P = {
            "category": "Planed",
            "start": new Date(value.Planed__St_Date),
            "end": new Date(value.Planed__En_Date),
            "color": colorSet.getIndex(key),
            "task": value.Task
        }

        var Data_A = {
            "category": "Actual",
            "start": new Date(value.Actual_St_Date),
            "end": new Date(value.Actual_En_Date),
            "color": colorSet.getIndex(key),
            "task": value.Task
        }
        Jason.Data_.push(Data_P);
        Jason.Data_.push(Data_A);
    });
    console.log(Jason.Data_);
    chart.data = Jason.Data_;
    //chart.data = [{
    //    "category": "Planed",
    //    "start": "2016-01-10",
    //    "end": "2016-01-13",
    //    "color": colorSet.getIndex(0),
    //    "task": "Gathering requirements"
    //}, {
    //        "category": "Planed",
    //        "start": "2016-02-05",
    //        "end": "2016-04-18",
    //        "color": colorSet.getIndex(1),
    //        "task": "Development"
    //    }, {
    //        "category": "Actual",
    //        "start": "2016-01-08",
    //        "end": "2016-10-10",
    //        "color": colorSet.getIndex(5),
    //        "task": "Gathering requirements"
    //    }
    //    ];
    //chart.data = [{
    //    "category": "Module #1",
    //    "start": "2016-01-10",
    //    "end": "2016-01-13",
    //    "color": colorSet.getIndex(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #1",
    //    "start": "2016-02-05",
    //    "end": "2016-04-18",
    //    "color": colorSet.getIndex(0),
    //    "task": "Development"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-08",
    //    "end": "2016-01-10",
    //    "color": colorSet.getIndex(5),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-12",
    //    "end": "2016-01-15",
    //    "color": colorSet.getIndex(5),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-16",
    //    "end": "2016-02-05",
    //    "color": colorSet.getIndex(5),
    //    "task": "Development"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-02-10",
    //    "end": "2016-02-18",
    //    "color": colorSet.getIndex(5),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "",
    //    "task": ""
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-01-01",
    //    "end": "2016-01-19",
    //    "color": colorSet.getIndex(9),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-02-01",
    //    "end": "2016-02-10",
    //    "color": colorSet.getIndex(9),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-03-10",
    //    "end": "2016-04-15",
    //    "color": colorSet.getIndex(9),
    //    "task": "Development"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-04-20",
    //    "end": "2016-04-30",
    //    "color": colorSet.getIndex(9),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-01-15",
    //    "end": "2016-02-12",
    //    "color": colorSet.getIndex(15),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-02-25",
    //    "end": "2016-03-10",
    //    "color": colorSet.getIndex(15),
    //    "task": "Development"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-03-23",
    //    "end": "2016-04-29",
    //    "color": colorSet.getIndex(15),
    //    "task": "Testing and QA"
    //}];
    //chart.exporting.menu = new am4core.ExportMenu();
    chart.dateFormatter.dateFormat = "yyyy-MM-dd";
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "category";
    categoryAxis.renderer.grid.template.disabled = true;
    categoryAxis.renderer.labels.template.paddingRight = 25;
    categoryAxis.renderer.minGridDistance = 10;
    categoryAxis.renderer.innerRadius = -60;
    categoryAxis.renderer.radius = 60;

    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 70;
    dateAxis.baseInterval = { count: 1, timeUnit: "day" };

    dateAxis.renderer.tooltipLocation = 0;
    dateAxis.startLocation = -0.5;
    dateAxis.renderer.line.strokeDasharray = "1,4";
    dateAxis.renderer.line.strokeOpacity = 0.7;
    dateAxis.tooltip.background.fillOpacity = 0.2;
    dateAxis.tooltip.background.cornerRadius = 5;
    dateAxis.tooltip.label.fill = new am4core.InterfaceColorSet().getFor("alternativeBackground");
    dateAxis.tooltip.label.paddingTop = 7;

    var labelTemplate = dateAxis.renderer.labels.template;
    labelTemplate.verticalCenter = "middle";
    labelTemplate.fillOpacity = 0.7;
    labelTemplate.background.fill = new am4core.InterfaceColorSet().getFor("background");
    labelTemplate.background.fillOpacity = 1;
    labelTemplate.padding(7, 7, 7, 7);

    var categoryAxisLabelTemplate = categoryAxis.renderer.labels.template;
    categoryAxisLabelTemplate.horizontalCenter = "left";
    categoryAxisLabelTemplate.adapter.add("rotation", function (rotation, target) {
        var position = dateAxis.valueToPosition(dateAxis.min);
        return dateAxis.renderer.positionToAngle(position) + 90;
    })

    var series1 = chart.series.push(new am4plugins_timeline.CurveColumnSeries());
    series1.columns.template.height = am4core.percent(20);
    series1.columns.template.tooltipText = "{task}: [bold]{openDateX}[/] - [bold]{dateX}[/]";

    series1.dataFields.openDateX = "start";
    series1.dataFields.dateX = "end";
    series1.dataFields.categoryY = "category";
    series1.columns.template.propertyFields.fill = "color"; // get color from data
    series1.columns.template.propertyFields.stroke = "color";
    series1.columns.template.strokeOpacity = 0;

    var bullet = new am4charts.CircleBullet();
    series1.bullets.push(bullet);
    bullet.circle.radius = 3;
    bullet.circle.strokeOpacity = 0;
    bullet.propertyFields.fill = "color";
    bullet.locationX = 0;


    var bullet2 = new am4charts.CircleBullet();
    series1.bullets.push(bullet2);
    bullet2.circle.radius = 3;
    bullet2.circle.strokeOpacity = 0;
    bullet2.propertyFields.fill = "color";
    bullet2.locationX = 1;

    chart.scrollbarX = new am4core.Scrollbar();
    chart.scrollbarX.align = "center"
    chart.scrollbarX.width = am4core.percent(90);

    var cursor = new am4plugins_timeline.CurveCursor();
    chart.cursor = cursor;
    cursor.xAxis = dateAxis;
    cursor.yAxis = categoryAxis;
    cursor.lineY.disabled = true;
    cursor.lineX.strokeDasharray = "1,4";
    cursor.lineX.strokeOpacity = 1;

    dateAxis.renderer.tooltipLocation2 = 0;
    categoryAxis.cursorTooltipEnabled = false;


}

function PlannedChart_ajax(ID, URL) {
    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: ID },
        success: function (data) {
            
           
            PlannedChart(data);
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
   
}
function PlannedChart(responce) {
    // Themes begin
    am4core.useTheme(am4themes_animated);
    // Themes end

    var chart = am4core.create("project_Plan", am4charts.XYChart);
    chart.hiddenState.properties.opacity = 0; // this creates initial fade-in

    chart.paddingRight = 30;
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd HH:mm";


    var Jason = {};
    var Data_ = []
    Jason.Data_ = Data_;

    var colorSet = new am4core.ColorSet();
    colorSet.saturation = 0.4;
    $.each(responce, function (key, value) {
        var Data_P = {
            "category": value.Phase,
            "start": new Date(value.Planed_StDate),
            "end": new Date(value.Planed_EnDate),
            "color": colorSet.getIndex(key),
            "task": value.Phase
        }
        Jason.Data_.push(Data_P);
    });
    console.log(Jason.Data_);
    chart.data = Jason.Data_;

    //chart.data = [{
    //    "category": "Module #1",
    //    "start": "2016-01-01",
    //    "end": "2016-01-14",
    //    "color": colorSet.getIndex(0).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #1",
    //    "start": "2016-01-16",
    //    "end": "2016-01-27",
    //    "color": colorSet.getIndex(0).brighten(0.4),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #1",
    //    "start": "2016-02-05",
    //    "end": "2016-04-18",
    //    "color": colorSet.getIndex(0).brighten(0.8),
    //    "task": "Development"
    //}, {
    //    "category": "Module #1",
    //    "start": "2016-04-18",
    //    "end": "2016-04-30",
    //    "color": colorSet.getIndex(0).brighten(1.2),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-08",
    //    "end": "2016-01-10",
    //    "color": colorSet.getIndex(2).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-12",
    //    "end": "2016-01-15",
    //    "color": colorSet.getIndex(2).brighten(0.4),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-01-16",
    //    "end": "2016-02-05",
    //    "color": colorSet.getIndex(2).brighten(0.8),
    //    "task": "Development"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-02-10",
    //    "end": "2016-02-18",
    //    "color": colorSet.getIndex(2).brighten(1.2),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-01-02",
    //    "end": "2016-01-08",
    //    "color": colorSet.getIndex(4).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-01-08",
    //    "end": "2016-01-16",
    //    "color": colorSet.getIndex(4).brighten(0.4),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-01-19",
    //    "end": "2016-03-01",
    //    "color": colorSet.getIndex(4).brighten(0.8),
    //    "task": "Development"
    //}, {
    //    "category": "Module #3",
    //    "start": "2016-03-12",
    //    "end": "2016-04-05",
    //    "color": colorSet.getIndex(4).brighten(1.2),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-01-01",
    //    "end": "2016-01-19",
    //    "color": colorSet.getIndex(6).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-01-19",
    //    "end": "2016-02-03",
    //    "color": colorSet.getIndex(6).brighten(0.4),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-03-20",
    //    "end": "2016-04-25",
    //    "color": colorSet.getIndex(6).brighten(0.8),
    //    "task": "Development"
    //}, {
    //    "category": "Module #4",
    //    "start": "2016-04-27",
    //    "end": "2016-05-15",
    //    "color": colorSet.getIndex(6).brighten(1.2),
    //    "task": "Testing and QA"
    //}, {
    //    "category": "Module #5",
    //    "start": "2016-01-01",
    //    "end": "2016-01-12",
    //    "color": colorSet.getIndex(8).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #5",
    //    "start": "2016-01-12",
    //    "end": "2016-01-19",
    //    "color": colorSet.getIndex(8).brighten(0.4),
    //    "task": "Producing specifications"
    //}, {
    //    "category": "Module #5",
    //    "start": "2016-01-19",
    //    "end": "2016-03-01",
    //    "color": colorSet.getIndex(8).brighten(0.8),
    //    "task": "Development"
    //}, {
    //    "category": "Module #5",
    //    "start": "2016-03-08",
    //    "end": "2016-03-30",
    //    "color": colorSet.getIndex(8).brighten(1.2),
    //    "task": "Testing and QA"
    //}];

    //chart.data = [{
    //    "category": "Module #1",
    //    "start": "2010-01-01",
    //    "end": "2010-01-14",
    //    "color": colorSet.getIndex(0).brighten(0),
    //    "task": "Gathering requirements"
    //}, {
    //    "category": "Module #2",
    //    "start": "2016-02-01",
    //    "end": "2016-02-14",
    //    "color": colorSet.getIndex(0).brighten(0),
    //    "task": "Gathering requirements"
    //}];
    //chart.exporting.menu = new am4core.ExportMenu();
    chart.dateFormatter.dateFormat = "yyyy-MM-dd";
    chart.dateFormatter.inputDateFormat = "yyyy-MM-dd";

    var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "category";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.inversed = true;

    var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
    dateAxis.renderer.minGridDistance = 70;
    dateAxis.baseInterval = { count: 1, timeUnit: "day" };
    // dateAxis.max = new Date(2018, 0, 1, 24, 0, 0, 0).getTime();
    //dateAxis.strictMinMax = true;
    dateAxis.renderer.tooltipLocation = 0;

    var series1 = chart.series.push(new am4charts.ColumnSeries());
    series1.columns.template.height = am4core.percent(70);
    series1.columns.template.tooltipText = "{task}: [bold]{openDateX}[/] - [bold]{dateX}[/]";

    series1.dataFields.openDateX = "start";
    series1.dataFields.dateX = "end";
    series1.dataFields.categoryY = "category";
    series1.columns.template.propertyFields.fill = "color"; // get color from data
    series1.columns.template.propertyFields.stroke = "color";
    series1.columns.template.strokeOpacity = 1;

    chart.scrollbarX = new am4core.Scrollbar();
}

function ReadinssChaeckOrStatus_ajax(ID, URL) {
    $.ajax({
        cache: false,
        async: false,
        type: "GET",
        url: URL,
        data: { Instance: ID },
        success: function (RS) {
            ReadinssChaeckOrStatusChart(RS)
        },
        error: function (a) {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
function ReadinssChaeckOrStatusChart(RS) {
    am4core.useTheme(am4themes_animated);
    // Themes end
    //if (Responce==1) {
    //    Responce = 100;
    //}
    var iconPath = "M421.976,136.204h-23.409l-0.012,0.008c-0.19-20.728-1.405-41.457-3.643-61.704l-1.476-13.352H5.159L3.682,74.507 C1.239,96.601,0,119.273,0,141.895c0,65.221,7.788,126.69,22.52,177.761c7.67,26.588,17.259,50.661,28.5,71.548  c11.793,21.915,25.534,40.556,40.839,55.406l4.364,4.234h206.148l4.364-4.234c15.306-14.85,29.046-33.491,40.839-55.406  c11.241-20.888,20.829-44.96,28.5-71.548c0.325-1.127,0.643-2.266,0.961-3.404h44.94c49.639,0,90.024-40.385,90.024-90.024  C512,176.588,471.615,136.204,421.976,136.204z M421.976,256.252h-32c3.061-19.239,5.329-39.333,6.766-60.048h25.234  c16.582,0,30.024,13.442,30.024,30.024C452,242.81,438.558,256.252,421.976,256.252z"

    var chart = am4core.create("Readiness_Check", am4charts.SlicedChart);
    chart.hiddenState.properties.opacity = 0; // this makes initial fade in effect
    //chart.paddingLeft = 150;

    //chart.data = [{
    //    "name": "B",
    //    "value": 4,
    //    "disabled": true
    //}];
    chart.data = [{
        "name": "Pending",
        "value": RS.Pending,
        "disabled": true
    },{
        "name": "Completed",
        "value": RS.Completed,
        "disabled": true
    }];

    var series = chart.series.push(new am4charts.PictorialStackedSeries());
    series.dataFields.value = "value";
    series.dataFields.category = "name";
    series.alignLabels = true;
    // this makes only A label to be visible
    series.labels.template.propertyFields.disabled = "disabled";
    series.ticks.template.propertyFields.disabled = "disabled";


    series.maskSprite.path = iconPath;
    series.ticks.template.locationX = 1;
    series.ticks.template.locationY = 0;

    series.labelsContainer.width = 5;

    chart.legend = new am4charts.Legend();
    chart.legend.position = "right";
    chart.legend.paddingRight = 5;
    chart.legend.paddingBottom = 5;
    let marker = chart.legend.markers.template.children.getIndex(0);
    chart.legend.markers.template.width = 10;
    chart.legend.markers.template.height = 10;
    marker.cornerRadius(5, 5, 5, 5);


}