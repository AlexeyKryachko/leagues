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
			"window.jQuery": "jquery"
		})
	],
	resolve: {
		alias: {
		   handlebars: 'handlebars/dist/handlebars.min.js'
		}
	}
};