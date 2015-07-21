({
    appDir: '../Content',
    dir: '../Release',
    baseUrl: "./js",
    mainConfigFile: '../Content/js/main.js',
    paths: {
        main: "../js/main"
    },
    keepBuildDir: false,
    modules: [
        //First set up the common build layer.
        {
            //module names are relative to baseUrl
            name: 'main',
            //List common dependencies here. Only need to list
            //top level dependencies, "include" will find
            //nested dependencies.
            include: ['jquery',
                      'bootstrap'

            ]
        },

        //Now set up a build layer for each page, but exclude
        //the common one. "exclude" will exclude
        //the nested, built dependencies from "common". Any
        //"exclude" that includes built modules should be
        //listed before the build layer that wants to exclude it.
        //"include" the appropriate "app/main*" module since by default
        //it will not get added to the build since it is loaded by a nested
        //requirejs in the page*.js files.
        {
            //module names are relative to baseUrl/paths config
            name: 'views/home/indexController',
            include: ['jquery'],
            exclude: ['main']
        },
        //Now set up a build layer for each page, but exclude
        //the common one. "exclude" will exclude
        //the nested, built dependencies from "common". Any
        //"exclude" that includes built modules should be
        //listed before the build layer that wants to exclude it.
        //"include" the appropriate "app/main*" module since by default
        //it will not get added to the build since it is loaded by a nested
        //requirejs in the page*.js files.
        {
            //module names are relative to baseUrl/paths config
            name: 'views/research/researchController',
            include: ['jquery'],
            exclude: ['main']
        }
    ]
})
