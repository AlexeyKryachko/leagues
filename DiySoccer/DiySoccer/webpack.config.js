var webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
    entry: {
        app: __dirname + "/content/entry.js",
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
        path: __dirname + '/content/dist/',
        filename: "[name].bundle.js"
    },
	/*module: {
	    rules: [
            {
                test: /\.css$/,
                loader: ExtractTextPlugin.extract("style-loader", "css-loader")
            },
            {
				test: /\.scss$/,
				loader: ExtractTextPlugin.extract(
                    'style', // The backup style loader
                    'css?sourceMap!sass?sourceMap'
                )
            },
		    {
                test: /\.woff(2)?(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "url-loader?limit=10000&minetype=application/font-woff"
		    },
		    {
		        test: /\.(ttf|eot|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "file-loader"
		    }
        ]
	},
	plugins: [
		new webpack.ProvidePlugin({
			$: "jquery",
			jQuery: "jquery",
			"window.jQuery": "jquery",
			_: "underscore"
		}),
		new ExtractTextPlugin('styles.bundle.css')
	],*/
	resolve: {
		modules: ['node_modules'],
		alias: {
			handlebars: 'handlebars/dist/handlebars.min.js',
			$: 'jquery/dist/jquery.min.js'
		}
	}
};