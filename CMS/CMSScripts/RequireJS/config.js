cmsrequire.config({
    baseUrl: '{%AppPath%}/CMSPages/GetResource.ashx?scriptmodule=',
    paths: {
        'jQuery': '{%AppPath%}/CMSScripts/jQuery/jquery-core',
        'jQueryUI': '{%AppPath%}/CMSScripts/Vendor/jQueryUI/jquery-ui-1.10.4.min',
        'Underscore': '{%AppPath%}/CMSScripts/Underscore/underscore.min',
        'jQueryJScrollPane': '{%AppPath%}/CMSScripts/jquery-jscrollpane',
        'jQueryFancySelector': '{%AppPath%}/CMSScripts/jquery/jquery-fancyselect',
        'jQueryDatePicker': '{%AppPath%}/CMSScripts/jquery/jquery-ui-datetimepicker',

        'angular': '{%AppPath%}/CMSScripts/Vendor/Angular/angular',
        'angular-1.4.5': '{%AppPath%}/CMSScripts/Vendor/Angular-1.4.5/angular',

        'angular-resource': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-resource.min',
        'angular-resource-1.4.5': '{%AppPath%}/CMSScripts/Vendor/Angular-1.4.5/angular-resource.min',

        'angular-sanitize': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-sanitize',
        'angular-sanitize-1.4.5': '{%AppPath%}/CMSScripts/Vendor/Angular-1.4.5/angular-sanitize',
        
        'angular-route': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-route.min',
        'angular-animate': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-animate',
        'angularSortable': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-sortable',
        'lodash': '{%AppPath%}/CMSScripts/Vendor/LoDash/lodash',
        'csv-parser': '{%AppPath%}/CMSScripts/Vendor/CSV-JS/csv',
        
        'amcharts': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/amcharts',
        'amcharts.cmstheme': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/Themes/CMSTheme',
        'amcharts.funnel': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/funnel',
        'amcharts.gauge': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/gauge',
        'amcharts.pie': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/pie',
        'amcharts.radar': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/radar',
        'amcharts.serial': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/serial',
        'amcharts.xy': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/xy',
        'amcharts.gantt': '{%AppPath%}/CMSScripts/CMSModules/CMS.Charts/amCharts/gantt',
        'angular-ellipsis': '{%AppPath%}/CMSScripts/Vendor/Angular/angular-ellipsis.min'
    },
    shim: {
        'jQuery': { exports: '$cmsj' },
        'jQueryUI': { deps: ['jQuery'] },
        'jQueryDatePicker': { deps: ['jQueryUI'] },
        'Underscore': { exports: '_' },
        'jQueryJScrollPane': { deps: ['jQuery'] },
        'jQueryFancySelector': { deps: ['jQuery'] },

        'angular': { exports: 'angular' },
        'angular-1.4.5': { exports: 'angular' },
        
        'angular-resource': { deps: ['angular'] },
        'angular-resource-1.4.5': { deps: ['angular-1.4.5'] },
        
        'angular-sanitize': { deps: ['angular'] },
        'angular-sanitize-1.4.5': { deps: ['angular-1.4.5'] },
        
        'angular-route': { deps: ['angular'] },
        'angular-animate': { deps: ['angular'] },
        'lodash': { exports: '_' },
        'angular-ellipsis': { deps:['angular']},
        'angularSortable': { deps: ['jQuery', 'jQueryUI', 'angular'] },
        'csv-parser': { exports: 'CSV' },

        'amcharts': {
            exports: 'AmCharts',
            init: function () {
                AmCharts.isReady = true;
            }
        },
        'amcharts.cmstheme': {
            deps: ['amcharts']
        },
        'amcharts.funnel': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.gauge': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.pie': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.radar': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.serial': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.xy': {
            deps: ['amcharts'],
            exports: 'AmCharts'
        },
        'amcharts.gantt': {
            deps: ['amcharts', 'amcharts.serial'],
            exports: 'AmCharts'
        }
    },

    priority: [
        'jQuery',
        'jQueryUI',
        'angular',
        'angular-route',
        'angular-animate',
        'angularSortable'
    ]
});