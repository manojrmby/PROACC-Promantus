

var Options = {
    responsive: true,

    legend: {
        display: false,
        position: "bottom",
        onclick: null
    },
    //tooltips: {
    //mode: 'label',
    //     callbacks: {
    //         label: function (tooltipItem, data) {

    //     	return data.datasets[tooltipItem.datasetIndex].label + ": " + numberWithCommas(tooltipItem.yLabel);
    //     }
    //     }
    //    },
    title: {
        display: false,
        text: "Activities (Move to HANA)"
    },
    //hover: {
    //	animationDuration: 0
    //            },
    plugins: {
        labels: {
            render: 'value'

        }
    },
    //animation: {
    //	duration: 5000,
    //	onComplete: function () {
    //		var chartInstance = this.chart,
    //		ctx = chartInstance.ctx;
    //		ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
    //		ctx.textAlign = 'center';
    //		ctx.textBaseline = 'bottom';
    //		var height = chartInstance.controller.boxes[0].bottom;
    //		this.data.datasets.forEach(function (dataset, i) {
    //			var meta = chartInstance.controller.getDatasetMeta(i);
    //			meta.data.forEach(function (bar, index) {
    //				var data = dataset.data[index];
    //				 ctx.fillStyle = '#333';
    //				ctx.fillText(data, bar._model.x, height - ((height - bar._model.y) / 2) - 10);

    //			});
    //		}
    //		);
    //	}
    //},
    scales: {
        xAxes: [{
            ticks: {
                display: false,//this will remove only the label
                beginAtZero: true
            },
            gridLines: {
                display: false,
                drawBorder: true,

            }
        }],
        yAxes: [{
            ticks: {
                display: false,//this will remove only the label
                beginAtZero: true
            },
            gridLines: {
                display: false,
                drawBorder: false,
            }
        }]
    }

}
//Tab Readiness
async function Rediness_SimplificationDonut(Id,URL) {
    $.ajax({
        cache: false,
        type: "GET",
        url: URL,
        data: { Instance: Id },
        success: function (Data) {
            data_ = [Data.R, Data.RC, Data.RC_NON, Data.R_NON];
            labels = ["Relevant", "Relevance to be Checked", "Relevance to be Checked (Non-strategic)", "Relevant (Non-strategic)"];

            SimplificationDonut(data_, labels);

        },
        error: function (e) {
            
            alert("Error while inserting data");
        }
    });
}
async function SimplificationDonut(data, labels) {
    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Relevant_Donut").getContext("2d");
    //var ctx = document.getElementById("Chart_Relevant").getContext("2d");
    var config = {
        type: "doughnut",
        data: {
            labels: labels,
            datasets: [{

                data: data,
                //backgroundColor: [gradientStroke_, gradientStroke_2],
                backgroundColor: [
                    '#71c989',//'#FF6384',
                    '#74abe2',
                    '#FFCE56',
                    '#f08956'
                ],
                borderWidth: 2,
                label: "Score"
            }]
        },
        options: {

            responsive: true,

            legend: {
                display: false,
                position: 'right'
            },
            title: {
                display: false,
                text: "Simplification"
            },
            centerPercentage: 60,
            centerArea: {
                displayText: true,
                text: "100%"
            },
            plugins: {
                labels: {
                    render: 'label' + 'value',
                    position: 'outside'
                }
                //labels: {
                //    render: 'label',
                //    //arc: true,
                //    fontColor: '#000',
                //    position: 'outside'
                //}
            },
            //scales: {
            //	xAxes: [{
            //		stacked: true
            //	}],
            //	yAxes: [{
            //		stacked: true
            //	}]
            //}
            animation: {
                duration: 5000
            }
            //                ,
            //scales: {
            //	xAxes: [{
            //		ticks: {
            //			display: false //this will remove only the label
            //		}
            //	}]
            //}
        }
    };

    window.myRadialGauge = new Chart(ctx, config);
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
                CustomCode_Bar(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function CustomCode_Bar(data, labels) {
    var maxval = Math.max.apply(Math, data);
    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_CustomCode").getContext("2d");



    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                backgroundColor: [
                    '#f08956',
                    '#36A2EB',
                    '#FFCE56'
                ],

            }]
        },

        // options: Options
        options: {
            responsive: true,

            legend: {
                display: false,
                position: "bottom"
            },
            plugins: {
                labels: {
                    render: 'value'
                }
            },
            //tooltips: {
            //mode: 'label',
            //     callbacks: {
            //         label: function (tooltipItem, data) {

            //     	return data.datasets[tooltipItem.datasetIndex].label + ": " + numberWithCommas(tooltipItem.yLabel);
            //     }
            //     }
            //    },
            title: {
                display: false,
                text: "Custom Code Analysis"
            },
            hover: {
                animationDuration: 0
            },
            //animation: {
            //	duration: 5000,
            //	onComplete: function () {
            //		var chartInstance = this.chart,
            //		ctx = chartInstance.ctx;
            //		ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
            //		ctx.textAlign = 'center';
            //		ctx.textBaseline = 'bottom';
            //		var height = chartInstance.controller.boxes[0].bottom;
            //		this.data.datasets.forEach(function (dataset, i) {
            //			var meta = chartInstance.controller.getDatasetMeta(i);
            //			meta.data.forEach(function (bar, index) {
            //				var data = dataset.data[index];
            //				 ctx.fillStyle = '#333';
            //				ctx.fillText(data, bar._model.x, height - ((height - bar._model.y) / 2) - 10);

            //			});
            //		}
            //		);
            //	}
            //},
            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                        //max: maxval+2
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false,

                    }
                }]
            }

        }


    };

    window.myRadialGauge = new Chart(ctx, config);
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
                Activities_Bar1(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Activities_Bar1(data, labels) {

    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Activities_Bar1").getContext("2d");



    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                label: "",

                backgroundColor: [
                    '#71c989',
                    '#36A2EB',
                    '#FFCE56',
                    '#f08956'
                ],

            }]
        },


        options: Options


    };
    window.myRadialGauge = new Chart(ctx, config);
    //ctx.height(500);
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

                    //var _Name = Output[i].Name;

                    //if (_Name == 'Custom Code Adaption') {
                    //	inpName.push(Output[i].Name);
                    //	inpVal.push(Output[i]._Value);
                    //} else if (_Name == 'Customizing / Configuration') {
                    //	inpName.push(Output[i].Name);
                    //	inpVal.push(Output[i]._Value);
                    //} else if (_Name == 'User Training') {
                    //	inpName.push(Output[i].Name);
                    //	inpVal.push(Output[i]._Value);
                    //} else {
                    //	C = C + Output[i]._Value;
                    //}
                }
                //inpName.push("Others");
                //inpVal.push(C);
                labels = inpName;
                data_ = inpVal;

                Activities_Donut(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Activities_Donut(data, labels) {

    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Activities_Donut").getContext("2d");

    //var gradientStroke_ = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_.addColorStop(0, "#FDB777");
    //gradientStroke_.addColorStop(1, "#FD9346");

    //var gradientStroke_2 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_2.addColorStop(0, "#FB291B");
    //gradientStroke_2.addColorStop(1, "#CD1B2D");

    //var gradientStroke_3 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_3.addColorStop(0, "#FDB777");
    //gradientStroke_3.addColorStop(1, "#FD9346");

    //var gradientStroke_4 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_4.addColorStop(0, "#FB291B");
    //gradientStroke_4.addColorStop(1, "#CD1B2D");

    var config = {
        //type: "horizontalBar",
        type: "doughnut",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                backgroundColor: [
                    '#71c989',
                    '#36A2EB',
                    '#FFCE56'
                ],
                //backgroundColor: [gradientStroke_, gradientStroke_2],
                //backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                //borderWidth: 2,
                //label: "Score"
            }]
        },
        options: {
            responsive: true,
            layout: {
                padding: {
                    top: 5
                }
            },
            responsive: true,
            legend: {

                display: false,
                position: 'right',
                maxWidth: 100,
                onClick: null
            },
            title: {
                display: false,
                text: "Condition"
            },
            centerPercentage: 60,
            centerArea: {
                displayText: true,
                text: "100%"
            },
            plugins: {
                labels: {
                    render: 'value',
                    position: 'outside'
                }
            },
            //title: {
            //                display: true,
            //	text: "Condition"
            //},
            //scales: {
            //	xAxes: [{
            //		stacked: true
            //	}],
            //	yAxes: [{
            //		stacked: true
            //	}]
            //}
            animation: {
                duration: 5000
            },


        }
    };
    window.myRadialGauge = new Chart(ctx, config);
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
                Activities_Bar2(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Activities_Bar2(data, labels) {
    var maxval = Math.max.apply(Math, data);
    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Activities_Bar2").getContext("2d");

    //var gradientStroke_ = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_.addColorStop(0, "#FDB777");
    //gradientStroke_.addColorStop(1, "#FD9346");

    //var gradientStroke_2 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_2.addColorStop(0, "#FB291B");
    //gradientStroke_2.addColorStop(1, "#CD1B2D");

    //var gradientStroke_3 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_3.addColorStop(0, "#FDB777");
    //gradientStroke_3.addColorStop(1, "#FD9346");

    //var gradientStroke_4 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_4.addColorStop(0, "#FB291B");
    //gradientStroke_4.addColorStop(1, "#CD1B2D");

    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                //backgroundColor: [gradientStroke_, gradientStroke_2],
                backgroundColor: [
                    '#71c989',
                    '#36A2EB',
                    '#FFCE56',
                    '#f08956'
                ],
                //borderWidth: 2,
                //label: "Score"
            }]
        },
        options: {

            responsive: true,
            legend: {
                display: false
            },
            title: {
                display: false,
                text: "Conversion Phase"
            },
            centerPercentage: 60,
            centerArea: {
                displayText: true,
                text: "100%"
            },
            plugins: {
                labels: {
                    render: 'value'
                }
            },
            //scales: {
            //	xAxes: [{
            //		stacked: true
            //	}],
            //	yAxes: [{
            //		stacked: true
            //	}]
            //}
            //animation: {
            //	duration: 1,
            //	onComplete() {
            //		const chartInstance = this.chart;
            //		const ctx = chartInstance.ctx;
            //		const dataset = this.data.datasets[0];
            //		const meta = chartInstance.controller.getDatasetMeta(0);

            //		Chart.helpers.each(meta.data.forEach((bar, index) => {

            //		//    this.data.datasets.forEach(function (dataset, i) {
            //		//    var meta = chartInstance.controller.getDatasetMeta(i);
            //		//    meta.data.forEach(function (bar, index) {
            //		//        var data = dataset.data[index];
            //		//        ctx.fillText(data, bar._model.x,height - ((height - bar._model.y) / 2) - 10);
            //		//    });
            //		//}
            //			var data = dataset.data[index];
            //		   // const label = this.data.labels[index] + ":" + data;
            //			const label = data;
            //			const labelPositionX = 20;
            //			const labelWidth = ctx.measureText(label).width + labelPositionX;

            //			ctx.textBaseline = 'middle';
            //			ctx.textAlign = 'left';
            //			ctx.fillStyle = '#333';
            //			ctx.fillText(label, labelPositionX, bar._model.y);
            //		}));
            //	}
            //},
            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false,
                    }
                }]
            }

            //scales: {
            //                xAxes: [{
            //                    display: false,
            //		ticks: {
            //			display: false //this will remove only the label
            //		}
            //	}],
            //                yAxes: [{
            //                    display: false,
            //                    ticks: {
            //                        beginAtZero: true,
            //			steps: 10,
            //			stepValue: 5,
            //			max: maxval +5						}
            //	}]
            //}
        }
    };
    window.myRadialGauge = new Chart(ctx, config);
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
                Fiori_Bar(data_, labels);
            }
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function Fiori_Bar(data, labels) {

    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Fiori_Bar1").getContext("2d");



    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                backgroundColor: [
                    '#71c989',
                    '#36A2EB',
                    '#FFCE56',
                    '#f08956'
                ],
                ////backgroundColor: [gradientStroke_, gradientStroke_2],
                //backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                //borderWidth: 2,
                //label: "Score"
            }]
        },
        options: {

            responsive: true,
            legend: {
                display: false
            },
            title: {
                display: false,
                text: "Role / LOB"
            },
            centerPercentage: 80,
            centerArea: {
                displayText: true,
                text: "100%"
            },
            plugins: {
                labels: {
                    render: 'value'

                }
            },
            //scales: {
            //	xAxes: [{
            //		stacked: true
            //	}],
            //	yAxes: [{
            //		stacked: true
            //	}]
            //}
            //animation: {
            //	duration: 1,
            //	onComplete() {
            //		const chartInstance = this.chart;
            //		const ctx = chartInstance.ctx;
            //		const dataset = this.data.datasets[0];
            //		const meta = chartInstance.controller.getDatasetMeta(0);

            //		Chart.helpers.each(meta.data.forEach((bar, index) => {

            //		//    this.data.datasets.forEach(function (dataset, i) {
            //		//    var meta = chartInstance.controller.getDatasetMeta(i);
            //		//    meta.data.forEach(function (bar, index) {
            //		//        var data = dataset.data[index];
            //		//        ctx.fillText(data, bar._model.x,height - ((height - bar._model.y) / 2) - 10);
            //		//    });
            //		//}
            //			var data = dataset.data[index];
            //		   // const label = this.data.labels[index] + ":" + data;
            //			const label = data;
            //			const labelPositionX = 20;
            //			const labelWidth = ctx.measureText(label).width + labelPositionX;

            //			ctx.textBaseline = 'middle';
            //			ctx.textAlign = 'left';
            //			ctx.fillStyle = '#333';
            //			ctx.fillText(label, labelPositionX, bar._model.y);
            //		}));
            //	}
            //},

            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false,
                    }
                }]
            }
        }
    };
    window.myRadialGauge = new Chart(ctx, config);
}



//Tab simplification
async function DropDown_Simp(Id,URL) {

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
async function Table_Simplification(Id,URL) {
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
async function SimplificationReport_Bar(Id,URL) {
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
            SimplificationReport_Bar_Chart(data_, labels, backColor);

        },
        error: function () {
            alert("Error while Getting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function SimplificationReport_Bar_Chart(data, labels, backColor) {
    $("canvas#Chart_Relevant").remove();

    $("#Chart123").append('<canvas id="Chart_Relevant" class="animated fadeIn"></canvas>');
    var ctx = document.getElementById("Chart_Relevant").getContext("2d");

    //var randomScalingFactor = function () {
    //	return Math.round(Math.random() * 100);
    //};


    var maxval = Math.max.apply(Math, data);

    Chart.defaults.global.defaultFontFamily = "Verdana";


    //var gradientStroke_ = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_.addColorStop(0, "#FDB777");
    //gradientStroke_.addColorStop(1, "#FD9346");

    //var gradientStroke_2 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_2.addColorStop(0, "#FB291B");
    //gradientStroke_2.addColorStop(1, "#CD1B2D");

    //var gradientStroke_3 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_3.addColorStop(0, "#FDB777");
    //gradientStroke_3.addColorStop(1, "#FD9346");

    //var gradientStroke_4 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_4.addColorStop(0, "#FB291B");
    //gradientStroke_4.addColorStop(1, "#CD1B2D");

    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [
                {

                    data: data,
                    backgroundColor: backColor,
                    //backgroundColor: [
                    //                      '#FF6384',
                    //                      '#36A2EB',
                    //                      '#FFCE56',
                    //	'#FF6384',
                    //                      '#36A2EB',
                    //                      '#FFCE56',
                    //	'#FF6384',
                    //                      '#36A2EB',
                    //                      '#FFCE56',
                    //	'#FF6384',
                    //                      '#36A2EB',
                    //                      '#FFCE56'
                    //                  ],
                    //backgroundColor: [gradientStroke_, gradientStroke_2],
                    //backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850","#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850","#8e5ea2"],
                    //borderWidth: 2,
                    //label: "Score"
                }
            ]
        },
        options: {

            responsive: true,
            legend: {
                display: false
            },
            title: {
                display: false,
                text: "Radial gauge chart"
            },
            centerPercentage: 60,
            centerArea: {
                displayText: true,
                text: "100%"
            },


            //animation: {
            //		duration:5000
            //},
            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    display: false,

                    ticks: {
                        beginAtZero: true,
                        steps: 10,
                        stepValue: 5,
                        //stepSize:.5,
                        //max: maxval + 5
                    }
                }]


            },
            plugins: {
                labels: {
                    render: 'value',

                },
                datalabels: {
                    align: 'end',
                    anchor: 'end',

                }
            }
        }
    };
    window.myRadialGauge = new Chart(ctx, config);

}


//Tab Activities
async function DropDown_Ass(Id,URL) {
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
                //var _Name = Output[i].Name;

                //if (_Name == 'Custom Code Adaption') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else if (_Name == 'Customizing / Configuration') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else if (_Name == 'User Training') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else {
                //	C = C + Output[i]._Value;
                //}
            }
            //inpName.push("Others");
            //inpVal.push(C);
            labels = inpName;
            data_ = inpVal;
            ActivitiesBar_Chart(data_, labels, backColor);
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function ActivitiesBar_Chart(data, labels, backColor) {
    $("canvas#Chart_Activities").remove();

    $("#ChartActivities").append('<canvas id="Chart_Activities"></canvas>');
    var ctx = document.getElementById("Chart_Activities").getContext("2d");

    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var ctx = document.getElementById("Chart_Activities").getContext("2d");



    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                label: "",
                backgroundColor: backColor,
                //backgroundColor: [
                //                       '#FF6384',
                //                       '#36A2EB',
                //                       '#FFCE56'
                //                   ],

            }]
        },


        options: {

            legend: {
                display: false,
                position: "bottom",
                onclick: null
            },
            //tooltips: {
            //mode: 'label',
            //     callbacks: {
            //         label: function (tooltipItem, data) {

            //     	return data.datasets[tooltipItem.datasetIndex].label + ": " + numberWithCommas(tooltipItem.yLabel);
            //     }
            //     }
            //    },
            title: {
                display: true,
                text: " "
            },
            //hover: {
            //	animationDuration: 0
            //            },

            //animation: {
            //	duration: 5000,
            //	onComplete: function () {
            //		var chartInstance = this.chart,
            //		ctx = chartInstance.ctx;
            //		ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
            //		ctx.textAlign = 'center';
            //		ctx.textBaseline = 'bottom';
            //		var height = chartInstance.controller.boxes[0].bottom;
            //		this.data.datasets.forEach(function (dataset, i) {
            //			var meta = chartInstance.controller.getDatasetMeta(i);
            //			meta.data.forEach(function (bar, index) {
            //				var data = dataset.data[index];
            //				 ctx.fillStyle = '#333';
            //				ctx.fillText(data, bar._model.x, height - ((height - bar._model.y) / 2) - 10);

            //			});
            //		}
            //		);
            //	}
            //},
            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: true
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false,
                    }
                }]
            },
            plugins: {
                labels: {
                    render: 'value',

                },
                datalabels: {
                    align: 'end',
                    anchor: 'end',

                }
            }

        }


    };
    window.myRadialGauge = new Chart(ctx, config);
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

                //var _Name = Output[i].Name;

                //if (_Name == 'Custom Code Adaption') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else if (_Name == 'Customizing / Configuration') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else if (_Name == 'User Training') {
                //	inpName.push(Output[i].Name);
                //	inpVal.push(Output[i]._Value);
                //} else {
                //	C = C + Output[i]._Value;
                //}
            }
            //inpName.push("Others");
            //inpVal.push(C);
            labels = inpName;
            data_ = inpVal;
            CustomBarChart(data_, labels, backColor);
        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {
        //chart.data = response;

    });
}
async function CustomBarChart(data, labels, backColor) {
    var maxval = Math.max.apply(Math, data);
    //Chart.defaults.global.defaultFontFamily = "Verdana";
    var maxval = Math.max.apply(Math, data);
    var ctx = document.getElementById("Chart_Custom_Code").getContext("2d");

    //var gradientStroke_ = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_.addColorStop(0, "#FDB777");
    //gradientStroke_.addColorStop(1, "#FD9346");

    //var gradientStroke_2 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_2.addColorStop(0, "#FB291B");
    //gradientStroke_2.addColorStop(1, "#CD1B2D");

    //var gradientStroke_3 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_3.addColorStop(0, "#FDB777");
    //gradientStroke_3.addColorStop(1, "#FD9346");

    //var gradientStroke_4 = ctx.createLinearGradient(500, 0, 100, 0);
    //gradientStroke_4.addColorStop(0, "#FB291B");
    //gradientStroke_4.addColorStop(1, "#CD1B2D");

    //var numberWithCommas = function(x) {
    //  return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //};
    //Chart.defaults.global.defaultFontColor = 'black';
    // Chart.defaults.global.defaultFontSize = 16;

    var config = {
        //type: "horizontalBar",
        type: "bar",

        data: {
            labels: labels,
            datasets: [{

                data: data,
                backgroundColor: backColor,
                //backgroundColor: [
                //                       '#FF6384',
                //                       '#36A2EB',
                //                       '#FFCE56'
                //                   ],
                //label:"",
                //backgroundColor: [gradientStroke_, gradientStroke_2],
                //backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                //hoverBackgroundColor='red'
                //           fill: true,
                //pointHoverRadius: 5,
                //pointHoverBackgroundColor: 'red'
                //borderWidth: 2,
                //label: "Score"
            }]
        },
        //options: {

        //	responsive: true,
        //	legend: {
        //		display: false
        //	},
        //	title: {
        //		display: false,
        //		text: "Radial gauge chart"
        //	},
        //	centerPercentage: 60,
        //	centerArea: {
        //		displayText: true,
        //		text: "100%"
        //	},
        //	//scales: {
        //	//	xAxes: [{
        //	//		stacked: true
        //	//	}],
        //	//	yAxes: [{
        //	//		stacked: true
        //	//	}]
        //	//}
        //	animation: {
        //		duration: 5000
        //	},
        //	scales: {
        //		xAxes: [{
        //			ticks: {
        //				display: false //this will remove only the label
        //			}
        //		}]
        //	}
        //}

        options: {

            legend: {
                display: false,
                position: "right",
                onClick: null
            },
            //tooltips: {
            //mode: 'label',
            //     callbacks: {
            //         label: function (tooltipItem, data) {

            //     	return data.datasets[tooltipItem.datasetIndex].label + ": " + numberWithCommas(tooltipItem.yLabel);
            //     }
            //     }
            //    },
            title: {
                display: false,
                text: "Custom Code Analysis"
            },
            hover: {
                animationDuration: 0
            },
            //animation: {
            //	duration: 5000,
            //	onComplete: function () {
            //		var chartInstance = this.chart,
            //		ctx = chartInstance.ctx;
            //		ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
            //		ctx.textAlign = 'center';
            //		ctx.textBaseline = 'bottom';
            //		var height = chartInstance.controller.boxes[0].bottom;
            //		this.data.datasets.forEach(function (dataset, i) {
            //			var meta = chartInstance.controller.getDatasetMeta(i);
            //			meta.data.forEach(function (bar, index) {
            //				var data = dataset.data[index];
            //				 ctx.fillStyle = '#333';
            //				ctx.fillText(data, bar._model.x, height - ((height - bar._model.y) / 2) - 10);

            //			});
            //		}
            //		);
            //	}
            //},
            //	scales: {
            //		xAxes: [{
            //			ticks: {
            //				display: false //this will remove only the label
            //			},
            //			gridLines: {
            //				display: false,
            //				drawBorder: false,
            //				 beginAtZero:true,
            //}
            //		}],
            //		yAxes: [{
            //			ticks: {
            //				display: false //this will remove only the label
            //			},
            //			gridLines: {
            //				display: false,
            //				drawBorder: false,
            //				beginAtZero: false,
            //			},
            //			ticks: {
            //			//	//beginAtZero: false,
            //			//	//steps: 10,
            //			//	//stepValue: 5,
            //				max: maxval + 10
            //			}
            //		}]
            //	}

            scales: {
                xAxes: [{
                    display: false,
                    ticks: {
                        display: false //this will remove only the label
                    }
                }],
                yAxes: [{
                    display: false,
                    ticks: {
                        beginAtZero: true,
                        steps: 10,
                        stepValue: 5,
                        max: maxval + 1
                    }
                }]
            },
            plugins: {
                labels: {
                    render: 'value',

                },
                datalabels: {
                    align: 'end',
                    anchor: 'end',

                }
            }
        }


    };
    window.myRadialGauge = new Chart(ctx, config);
}

//Tab Fiori
async function DropDown_Fiori(Id,URL) {
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
            //renderChart(data_, labels, backColor);
            Fiori_Chart(data_, labels, backColor);

        },
        error: function () {
            alert("Error while inserting data");
        }
    }).done(function (response) {

        //chart.data = response;

    });
}
async function Fiori_Chart(data, labels, backColor) {
    //var rectangleSet = false;
    var maxval = Math.max.apply(Math, data);
    $(".chartAreaWrapper").remove();

    // debugger;
    $("#chartWrapper").append('<div class="chartAreaWrapper col-md-12"><div class="chartAreaWrapper2"><canvas id="chart-Test"></canvas></div><canvas id="axis-Test"></canvas></div>');
    if ($('#axis-Test').length > 0) { } else { $('#chartWrapper').append('<canvas id="axis-Test"></canvas>'); }
    //var ctx = document.getElementById("chart-Test").getContext("2d");
    var canvasTest = $('#chart-Test');
    //canvasTest.height = 500;
    var chartTest = new Chart(canvasTest, {
        //type: "horizontalBar",
        type: 'bar',
        borderWidth: 2,
        data: {
            labels: labels,
            datasets: [
                {
                    data: data,
                    backgroundColor: backColor,
                }
            ]
        },

        options: {
            animation: {
                duration: 5000
            },
            responsive: true,
            legend: {
                display: false
            },
            title: {
                display: false,
                text: "Radial gauge chart"
            },
            centerPercentage: 10,
            centerArea: {
                displayText: true,
                text: "100%"
            },

            scales: {
                xAxes: [{
                    ticks: {
                        display: false,//this will remove only the label
                        beginAtZero: false,

                        //stepSize:.5,
                        max: maxval + 5,

                    },
                    gridLines: {
                        display: false,
                        drawBorder: true,

                    }
                }],
                yAxes: [{
                    display: false,

                    ticks: {
                        beginAtZero: true,

                        //stepSize:.5,
                        max: maxval + 5,

                    }
                    ,
                    gridLines: {
                        display: true,
                        drawBorder: true,

                    }
                }]


            },
            plugins: {
                labels: {
                    render: 'value',

                },
                datalabels: {
                    align: 'end',
                    anchor: 'end',

                }
            }

            //animation: {
            //    onComplete: function () {
            //if (!rectangleSet) {
            //    var scale = window.devicePixelRatio;
            //    debugger;
            //    var sourceCanvas = chartTest.chart.canvas;
            //    var copyWidth = chartTest.scales['y-axis-0'].width - 10;
            //    var copyHeight = chartTest.scales['y-axis-0'].height + chartTest.scales['y-axis-0'].top +10;

            //    var targetCtx = document.getElementById("axis-Test").getContext("2d");

            //    targetCtx.scale(scale, scale);
            //    targetCtx.canvas.width = copyWidth * scale;
            //    targetCtx.canvas.height = copyHeight * scale;

            //    targetCtx.canvas.style.width = `${copyWidth}px`;
            //    targetCtx.canvas.style.height = `${copyHeight}px`;
            //    targetCtx.drawImage(sourceCanvas, 0, 0, copyWidth * scale, copyHeight * scale, 0, 0, copyWidth * scale, copyHeight * scale);

            //    var sourceCtx = sourceCanvas.getContext('2d');

            //    // Normalize coordinate system to use css pixels.

            //    sourceCtx.clearRect(0, 0, copyWidth * scale, copyHeight * scale);
            //    rectangleSet = true;
            //}
            //}
            //,
            //onProgress: function () {
            //    if (rectangleSet === true) {
            //        var copyWidth = chartTest.scales['y-axis-0'].width;
            //        var copyHeight = chartTest.scales['y-axis-0'].height + chartTest.scales['y-axis-0'].top + 10;

            //        var sourceCtx = chartTest.chart.canvas.getContext('2d');
            //        sourceCtx.clearRect(0, 0, copyWidth, copyHeight);
            //    }
            //}
            //}
        }
    });
    addData(data.length, chartTest);

}
async function addData(numData, chart) {

    for (var i = 0; i < numData; i++) {
        //chart.data.datasets[0].data.push(1 * 100);
        //chart.data.labels.push("Label" + i);
        if (numData == 1) {

            //$(".chartAreaWrapper").remove();
            $('#Chart123').removeClass('col-md-12').addClass('col-md-6');
            $('#axis-Test').remove();
        }

        if (numData > 1) {
            $('#Chart123').removeClass('col-md-6').addClass('col-md-12');

            var newwidth = $('.chartAreaWrapper2').width() + 15;
            $('.chartAreaWrapper2').width(newwidth);
            $('.chartAreaWrapper2').height("200px")
        }

    }
    $('.chartAreaWrapper').scrollTop($('.chartAreaWrapper')[0].scrollHeight);

}
