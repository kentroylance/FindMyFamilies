



({
    baseUrl: './',
    appDir: './js',
    dir: './js-built',
    mainConfigFile: './js/main.js',
    paths: {
        'jquery': 'empty:',
        'knockout': 'empty:',
        'bootstrap': 'empty:'
    },

    modules: [{
            name: 'main'  

        }, {
            name: 'lib/jquery/package'

        }, {
            name: 'lib/knockout/package'


        }
     ]
})
