function DrawChart(idDiv, Title, Data) {
    if (Data !== null && Data != "") {

        Highcharts.setOptions({
            chart: {
                style: {
                    fontFamily: "'Open Sans', sans-serif"
                }
            }
        });

        Highcharts.chart(idDiv, {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: Title 
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    }
                }
            },
            series: [{
                name: 'Percentage',
                colorByPoint: true,
                data: formatData(Data)
            }]
        });
    }
}


function FormatData(Data) {
    var DataRetour = [];
    var isFirst = true;
    Data.forEach(function (element) {
        var myElement = null;
        if (isFirst) {
            myElement = {
                name: '[[[' + element[0] + ']]]',
                y: parseFloat(element[1]),
                sliced: true,
                selected: true
            };
        }
        else {
            myElement = {
                name: element[0],
                y: parseFloat(element[1])
            };
        }


        DataRetour.push(myElement);
        isFirst = false;
    });

    return DataRetour;
}