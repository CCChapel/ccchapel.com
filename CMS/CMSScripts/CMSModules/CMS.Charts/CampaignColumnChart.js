cmsdefine(['amcharts.serial', 'amcharts.cmstheme'], function (chart) {
    'use strict';

    var Module = function (config) {
        var resizeChartsColumnsAndLegend = function(chart) {
                // Pixel value reserved for each category, it represents column width and padding (see less variable @column-width-with-padding).
                var categoryWidth = 176;
                var width = chart.dataProvider.length * categoryWidth;

                document.getElementById(config.chartDiv).style.width = "" + width + "px";
                document.getElementById(config.legendDiv).style.width = "" + width + "px";

                // Since the chart is already build for the initial container size we need to
                // revalidate it's size. we'll delay a little bit the call to invalidateSize()
                // so that the chart elements are created first.
                setTimeout(function () {
                    chart.invalidateSize();
                    document.getElementById(config.chartDiv).style.display = "block";
                }, 1);
            },
            createLegend = function (chart) {
                // Populates custom legend when chart renders.
                chart.customLegend = document.getElementById(config.legendDiv);
                for (var i in chart.dataProvider) {
                    var row = chart.dataProvider[i];
                    legend.innerHTML += '<div class="legend-item">' +
                                            '<strong>' + row.percent + ' %</strong> (' + row.formattedValue + ')' +
                                        '</div>';
                }
            };

        chart.addInitHandler(function (chart) {
            resizeChartsColumnsAndLegend(chart);
            createLegend(chart);
        }, ['serial']);

        chart.makeChart(config.chartDiv, {
            type: "serial",
            theme: "CMSTheme",
            autoMargins: false,
            marginLeft: 0,
            marginRight: 0,
            marginBottom: 0,
            marginTop: 0,
            dataProvider: config.data,
            gridAboveGraphs: false,
            startDuration: 1,
            graphs: [{
                balloonText: "[[title]]",
                fillAlphas: 0.8,
                lineAlpha: 0.2,
                type: "column",
                valueField: "value",
                columnWidth: 0.9,
            }],
            chartCursor: {
                categoryBalloonEnabled: false,
                cursorAlpha: 0,
                zoomable: false
            },
            valueAxes: [{
                labelsEnabled: false,
                gridAlpha: 0,
                gridColor: "#FFFFFF",
                axisColor: "#FFFFFF",
                autoGridCount: false,
                minimum: 0,
                maximum: config.maxValue,
            }],
            categoryField: "title",
            categoryAxis: {
                gridPosition: "start",
                tickPosition: "start",
                gridAlpha: 0,
                gridColor: "#FFFFFF",
                axisColor: "#FFFFFF",
                autoWrap: true,
                fontSize: 12,
                labelsEnabled: false,
            },
        });
    };

    return Module;
});