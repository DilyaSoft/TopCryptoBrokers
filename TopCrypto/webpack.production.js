"use strict";

const path = require('path');

const ExtractTextPlugin = require('extract-text-webpack-plugin');

module.exports = {
  entry: {
    "css/topCryptoroot.css": "./src/css/root/app.scss",
    "js/topCryptoroot.js": './src/js/topCryptoRoot/topCryptoroot.ts',
    "css/gentelella.css": "./src/css/gentelella.scss",
    "css/login.css": './src/css/login/login.scss',
    "js/login.js": './src/js/loginApp/loginApp.ts',
    "css/adminApp.css": './src/css/adminRoot/adminRoot.scss',
    "js/adminApp.js": './src/js/adminApp/adminApp.ts',
  },
  output: {
    filename: '[name]',
    path: __dirname + '/wwwroot'
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', 'scss', 'css', 'png', 'woff', 'woff2', 'eot', 'ttf', 'svg']
  },
  devtool: 'nosources-source-map',
  module: {
    rules: [
      {
        test: /\.tsx?$/
        , loader: 'babel-loader?presets[]=es2015!ts-loader'
        , exclude: /(node_modules|bower_components|wwwroot)/
      },
      {
        test: /\.scss$/
        , loader: ExtractTextPlugin.extract({
          fallback: 'style-loader'
          , use: [{ loader: 'css-loader', options: { minimize: true } }, 'sass-loader']
        })
        , exclude: /(node_modules|bower_components|wwwroot)/
      },
      {
        test: /\.(png|woff|woff2|eot|ttf|svg)(\?v=[0-9]\.[0-9]\.[0-9])?$/,
        loader: 'url-loader?limit=100000'
      }
    ]
  },
  plugins: [
    new ExtractTextPlugin({
      filename: "[name]"
    }),
  ]
};

