/// <binding ProjectOpened='Watch - Development' />
"use strict";

module.exports = function () {
  return require(`./webpack.${process.env.NODE_ENV}.js`);
};