(function() {
    require.config({
//        urlArgs: "bust=" + (new Date()).getTime(),
        waitSeconds: 200,
        paths: {
            jquery: 'vendor/jquery1.11.1',
            jqueryui: 'vendor/jquery-ui-1.11.2',
            jqueryUiOptions: 'lib/jqueryUiOptions',
            formValidation: 'vendor/formvalidation/formValidation.min',
            bootstrapValidation: 'vendor/formvalidation/framework/bootstrap.min',
            underscore: 'vendor/underscore-min',
            greensock: 'vendor/greensock',
            transitions: 'vendor/layerslider.transitions',
            layerslider: 'vendor/layerslider.kreaturamedia.jquery',
            bootstrap: 'vendor/bootstrap3.2.0',
            spin: 'vendor/spin.min',
            fancybox: 'vendor/fancybox/jquery.fancybox.pack',
            fancyboxMedia: 'vendor/fancybox/helpers/jquery.fancybox-media',
            normalize: 'vendor/normalize',
            lazyload: 'vendor/jquery.lazyload',
            string: 'vendor/string.min',
            lazyRequire: 'lib/lazyRequire',
            _layoutController: 'views/shared/_layoutController',
            _layoutEvents: 'views/shared/_layoutEvents',
            msgBox: 'lib/msgBox',
            constants: 'lib/constants',
            system: 'lib/system',
            researchController: 'views/research/researchController',
            research: 'views/research/research',
            startingPoint: 'views/research/startingPoint',
            startingPointController: 'views/research/startingPointController',
            startingPointReport: 'views/research/startingPointReport',
            startingPointReportController: 'views/research/startingPointReportController',
            possibleDuplicates: 'views/research/possibleDuplicates',
            possibleDuplicatesController: 'views/research/possibleDuplicatesController',
            possibleDuplicatesReport: 'views/research/possibleDuplicatesReport',
            possibleDuplicatesReportController: 'views/research/possibleDuplicatesReportController',
            hints: 'views/research/hints',
            hintsController: 'views/research/hintsController',
            hintsReport: 'views/research/hintsReport',
            hintsReportController: 'views/research/hintsReportController',
            feedback: 'views/research/feedback',
            feedbackController: 'views/research/feedbackController',
            features: 'views/research/features',
            featuresController: 'views/research/featuresController',
            dateProblems: 'views/research/dateProblems',
            dateProblemsController: 'views/research/dateProblemsController',
            dateProblemsReport: 'views/research/dateProblemsReport',
            dateProblemsReportController: 'views/research/dateProblemsReportController',
            placeProblems: 'views/research/placeProblems',
            placeProblemsController: 'views/research/placeProblemsController',
            placeProblemsReport: 'views/research/placeProblemsReport',
            placeProblemsReportController: 'views/research/placeProblemsReportController',
            ordinances: 'views/research/ordinances',
            ordinancesController: 'views/research/ordinancesController',
            ordinancesReport: 'views/research/ordinancesReport',
            ordinancesReportController: 'views/research/ordinancesReportController',
            findClues: 'views/research/findClues',
            findCluesController: 'views/research/findCluesController',
            findCluesReport: 'views/research/findCluesReport',
            findCluesReportController: 'views/research/findCluesReportController',
            findPersonController: 'views/research/findPersonController',
            findPerson: 'views/research/findPerson',
            findPersonOptionsController: 'views/research/findPersonOptionsController',
            findPersonOptions: 'views/research/findPersonOptions',
            findPersonHelper: 'views/research/findPersonHelper',
            personUrlOptions: 'views/research/personUrlOptions',
            personUrlOptionsController: 'views/research/personUrlOptionsController',
            personUrls: 'views/research/personUrls',
            personUrlsController: 'views/research/personUrlsController',
            researchHelper: 'views/research/researchHelper',
            person: 'views/shared/person',
            retrieve: 'views/research/retrieve',
            retrieveController: 'views/research/retrieveController',
            indexController: 'views/home/indexController',
            hoverIntent: 'lib/jquery.hoverIntent.min'
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
            hoverIntent: {
                exports: 'hoverIntent'
            },
            findPerson: {
                exports: 'findPerson'
            },
            possibleDuplicates: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'possibleDuplicates'
            },
            dateProblems: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'dateProblems'
            },
            placeProblems: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'placeProblems'
            },
            hints: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'hints'
            },
            feedback: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'feedback'
            },
            features: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'features'
            },
            ordinances: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'ordinances'
            },
            startingPointReport: {
                exports: 'startingPointReport'
            },
            possibleDuplicatesReport: {
                exports: 'possibleDuplicatesReport'
            },
            hintsReport: {
                exports: 'hintsReport'
            },
            dateProblemsReport: {
                exports: 'dateProblemsReport'
            },
            placeProblemsReport: {
                exports: 'placeProblemsReport'
            },
            ordinancesReport: {
                exports: 'ordinancesReport'
            },
            findCluesReport: {
                exports: 'findCluesReport'
            },
            findPersonOptions: {
                exports: 'findPersonOptions'
            },
            personUrlOptions: {
                exports: 'personUrlOptions'
            },
            personUrls: {
                exports: 'personUrls'
            },
            formValidation: { deps: ['jquery'], exports: "FormValidation" },
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
                deps: ['jquery'],
                exports: '_layoutController'
            },
            indexController: {
                deps: ['jquery', 'system'],
                exports: 'indexController'
            },
            fancybox: {
                deps: ['jquery'],
                exports: 'fancybox'
            },
            fancyboxMedia: {
                deps: ['jquery', 'fancybox'],
                exports: 'fancyboxMedia'
            },
            person: {
                deps: ['jquery', 'system', 'constants'],
                exports: 'person'
            },
            startingPoint: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'startingPoint'
            },
            findClues: {
                deps: ['jquery', 'person', 'constants'],
                exports: 'findClues'
            },
            researchController: {
                deps: ['jquery', 'system'],
                exports: 'researchController'
            },
            startingPointController: {
                deps: ['jquery', 'startingPoint', 'startingPointReport', 'hoverIntent', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'startingPointController'
            },
            findCluesController: {
                deps: ['jquery', 'findClues', 'findCluesReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'findCluesController'
            },
            retrieveController: {
                deps: ['jquery', 'retrieve', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'retrieveController'
            },
            startingPointReportController: {
                deps: ['jquery', 'startingPointReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'startingPointReportController'
            },
            findCluesReportController: {
                deps: ['jquery', 'findCluesReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'findCluesReportController'
            },
            possibleDuplicatesReportController: {
                deps: ['jquery', 'possibleDuplicatesReport', 'researchHelper', 'msgBox', 'system','constants', 'person', 'lazyload'],
                exports: 'possibleDuplicatesReportController'
            },
            possibleDuplicatesController: {
                deps: ['jquery', 'possibleDuplicates', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'possibleDuplicatesController'
            },
            hintsReportController: {
                deps: ['jquery', 'hintsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'hintsReportController'
            },
            hintsController: {
                deps: ['jquery', 'hints', 'hintsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'hintsController'
            },
            feedbackController: {
                deps: ['jquery', 'formValidation', 'bootstrapValidation', 'feedback', 'researchHelper', 'system', 'constants'],
                exports: 'feedbackController'
            },
            featuresController: {
                deps: ['jquery', 'features', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'featuresController'
            },
            dateProblemsReportController: {
                deps: ['jquery', 'dateProblemsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'dateProblemsReportController'
            },
            dateProblemsController: {
                deps: ['jquery', 'dateProblems', 'dateProblemsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'dateProblemsController'
            },
            placeProblemsReportController: {
                deps: ['jquery', 'placeProblemsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'dateProblemsReportController'
            },
            placeProblemsController: {
                deps: ['jquery', 'placeProblems', 'placeProblemsReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'placeProblemsController'
            },
            ordinancesReportController: {
                deps: ['jquery', 'ordinancesReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'ordinancesReportController'
            },
            ordinancesController: {
                deps: ['jquery', 'ordinances', 'ordinancesReport', 'researchHelper', 'msgBox', 'system', 'constants', 'person', 'lazyload'],
                exports: 'ordinancesController'
            },
            findPersonController: {
                deps: ['jquery', 'hoverIntent', 'formValidation', 'bootstrapValidation', 'findPerson', 'findPersonHelper', 'system', 'constants', 'person', 'lazyload', 'string'],
                exports: 'findPersonController'
            },
            findPersonOptionsController: {
                deps: ['jquery', 'findPersonOptions', 'findPersonHelper', 'findPerson', 'system', 'constants', 'person', 'lazyload', 'string'],
                exports: 'findPersonOptionsController'
            },
            personUrlOptionsController: {
                deps: ['jquery', 'personUrlOptions', 'system', 'constants', 'person', 'lazyload', 'string'],
                exports: 'personUrlOptionsController'
            },
            personUrlsController: {
                deps: ['jquery', 'hoverIntent', 'personUrls', 'system', 'constants', 'person', 'lazyload', 'string'],
                exports: 'personUrlsController'
            },
            findPersonHelper: {
                deps: ['jquery', 'researchHelper', 'system', 'constants', 'person', 'string'],
                exports: 'findPersonHelper'
            },
            researchHelper: {
                deps: ['jquery', 'system', 'findPerson', 'retrieve', 'features', 'feedback', 'constants', 'person', 'string'],
                exports: 'researchHelper'
            },
            lazyload: ["jquery"],
            system: {
                deps: ["jquery", "constants"],
                exports: 'system'
            },
            retrieve: {
                deps: ["jquery", "system", "person"]
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
//            "css", //require plugins
            "jquery",
//            "css!/Content/css/lib/common/bootstrap3.2.0",
//            "css!/Content/css/lib/common/font-awesome-4.3.0/css/font-awesome.min",
//            "css!/Content/Template/css/style",
            "bootstrap",
//            "bootstrapTable",
            "_layoutController", // app lib modules
            "system"
//            "formValidation",
//            "bootstrapValidation"

        ],
        function() {
        });
}());