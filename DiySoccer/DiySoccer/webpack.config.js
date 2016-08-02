var webpack = require('webpack')

module.exports = {
    entry: {
		app : "./content/js/app/entry.js",
		vendor : [
			'jquery',
			'jquery-deferred',
			'backbone.marionette',
			'handlebars',
			'underscore',
			'bootstrap-3-typeahead',
			'jquery-datetimepicker'
		]		
	},
    output: {
        path: './content/dist/js/',
        filename: "[name].bundle.js"
    },
	plugins: [
		new webpack.ProvidePlugin({
			$: "jquery",
			jQuery: "jquery",
			"window.jQuery": "jquery",
			_: "underscore",
		})
	],
	resolve: {
		modulesDirectories: ['node_modules'],
		alias: {
			handlebars: 'handlebars/dist/handlebars.min.js',
			$: 'jquery/dist/jquery.min.js'
		}
	}
};