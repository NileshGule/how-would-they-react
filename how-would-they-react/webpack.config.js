// const NodePolyfillPlugin = require("node-polyfill-webpack-plugin");

module.exports = {
  // Other webpack configuration options...
  resolve: {
    fallback: {
      fs: require("fs"),
      tls: false,
      net: false,
      path: require.resolve("path-browserify"),
      zlib: false,
      http: false,
      https: false,
      stream: false,
      crypto: require.resolve("crypto-browserify"),
      os: require.resolve("os-browserify/browser"),
      // Add any other core modules you need here
    },
  },
  // plugins: [
  //   new NodePolyfillPlugin()    
  // ],
  // Other webpack configuration...
  mode: 'development',
};
