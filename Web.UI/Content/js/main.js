(function() {
    require.config({
//        urlArgs: "bust=" + (new Date()).getTime(),
        waitSeconds: 200,
        paths: {
            jquery: 'vendor/jquery1.11.1',
            jqueryui: 'vendor/jquery-ui-1.11.2',
            jqueryUiOptions: 'lib/jqueryUiOptions',
            bootstrapTable: 'vendor/bootstrap-table.min',
            formValidation: 'vendor/formvalidation/formValidation.min',
            bootstrapValidation: 'vendor/formvalidation/framework/bootstrap.min',
            domReady: 'vendor/domReady',
            underscore: 'vendor/underscore-min',
            greensock: 'vendor/greensock',
            transitions: 'vendor/layerslider.transitions',
            layerslider: 'vendor/layerslider.kreaturamedia.jquery',
            bootstrap: 'vendor/bootstrap3.2.0',
            spin: 'vendor/spin.min',
            fancybox: 'vendor/jquery.fancybox.pack',
            normalize: 'vendor/normalize',
            lazyload: 'vendor/jquery.lazyload',
            string: 'vendor/string.min',
            lazyRequire: 'lib/lazyRequire',
            _layoutController: 'views/shared/_layoutController',
            _layoutEvents: 'views/shared/_layoutEvents',
            msgBox: 'lib/msgBox',
            constants: 'lib/constants',
            user: 'views/shared/user',
            system: 'lib/system',
            researchController: 'views/research/researchController',
            research: 'views/research/research',
            startingPoint: 'views/research/startingPoint',
            startingPointController: 'views/research/startingPointController',
            startingPointReport: 'views/research/startingPointReport',
            startingPointReportController: 'views/research/startingPointReportController',
            possibleDuplicatesController: 'views/research/possibleDuplicatesController',
            possibleDuplicates: 'views/research/possibleDuplicates',
            findPersonController: 'views/research/findPersonController',
            findPerson: 'views/research/findPerson',
            findPersonOptionsController: 'views/research/findPersonOptionsController',
            findPersonOptions: 'views/research/findPersonOptions',
            findPersonHelper: 'views/research/findPersonHelper',
            researchHelper: 'views/research/researchHelper',
            person: 'views/shared/person',
            retrieve: 'views/research/retrieve',
            indexController: 'views/home/indexController'
        },
        map: {
            '*': {
                css: 'vendor/css.min'
            }
        },
        shim: {
            jquery: {
                exports: '$'
            },
            string: {
                exports: 'string'
            },
            findPerson: {
                exports: 'findPerson'
            },
            possibleDuplicates: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'possibleDuplicates'
            },
            startingPointReport: {
                exports: 'startingPointReport'
            },
            findPersonOptions: {
                exports: 'findPersonOptions'
            },
            formValidation: { deps: ['jquery'], exports: "FormValidation" },
            bootstrapTable: { deps: ["jquery", "bootstrap"] },
            bootstrapValidation: { deps: ["jquery", "bootstrap", "formValidation"] },
            layerslider: ["jquery", "greensock", "transitions"],
            jqueryui: {
                deps: ['jquery'],
                exports: "jQuery.ui"
            },
            bootstrap: {
                deps: ['jquery'],
                exports: "$"
            },
            _layoutController: {
                deps: ['jquery', 'bootstrap', 'system'],
                exports: '_layoutController'
            },
            fancybox: {
                deps: ['jquery'],
                exports: 'fancybox'
            },
            person: {
                deps: ['jquery', 'system', 'user', 'constants'],
                exports: 'person'
            },
            user: {
                deps: ['jquery', 'constants'],
                exports: 'user'
            },
            startingPoint: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'startingPoint'
            },
            researchController: {
                deps: ['jquery', 'system', 'user', 'constants', 'person', 'domReady', 'lazyload', 'research', 'researchHelper'],
                exports: 'researchController'
            },
            startingPointController: {
                deps: ['jquery', 'startingPoint', 'startingPointReport', 'research', 'researchHelper', 'msgBox', 'system', 'user', 'constants', 'person', 'domReady', 'lazyload'],
                exports: 'startingPointController'
            },
            startingPointReportController: {
                deps: ['jquery', 'startingPointReport', 'system', 'constants', 'person'],
                exports: 'startingPointReportController'
            },
            possibleDuplicatesController: {
                deps: ['jquery', 'possibleDuplicates', 'research', 'msgBox', 'system', 'user', 'constants', 'person', 'domReady', 'lazyload'],
                exports: 'possibleDuplicatesController'
            },
            findPersonController: {
                deps: ['jquery', 'formValidation', 'bootstrapValidation', 'bootstrapTable', 'findPerson', 'findPersonHelper', 'system', 'user', 'constants', 'person', 'domReady', 'lazyload', 'string'],
                exports: 'findPersonController'
            },
            findPersonOptionsController: {
                deps: ['jquery', 'findPersonOptions', 'findPersonHelper', 'findPerson', 'system', 'user', 'constants', 'person', 'domReady', 'lazyload', 'string'],
                exports: 'findPersonOptionsController'
            },
            findPersonHelper: {
                deps: ['jquery', 'system', 'user', 'constants', 'person', 'string'],
                exports: 'findPersonHelper'
            },
            researchHelper: {
                deps: ['jquery', 'system', 'user', 'findPerson', 'research', 'constants', 'person', 'string'],
                exports: 'researchHelper'
            },
            lazyload: ["jquery"],
            system: {
                deps: ["jquery", "user", "constants"],
                exports: 'system'
            },
            retrieve: {
                deps: ["jquery", "system", "person", "user"]
            },
            msgBox: {
                deps: ["jquery", "jqueryui", "jqueryUiOptions"]
            }
        }
    });
    //libs
//    require(["jquery", "bootstrap"],
//        function ($, bootstrap) {
//            console.log("Test output");
//            console.log("$: " + typeof $);
//        }
//    );
    require([
            "css", //require plugins
            "jquery",
//            "css!/Content/css/lib/common/bootstrap3.2.0",
//            "css!/Content/css/lib/common/font-awesome-4.3.0/css/font-awesome.min",
//            "css!/Content/Template/css/style",
            "bootstrap",
//            "bootstrapTable",
            "_layoutController", // app lib modules
            "user",
            "system"
//            "formValidation",
//            "bootstrapValidation"

        ],
        function() {
        });
}());