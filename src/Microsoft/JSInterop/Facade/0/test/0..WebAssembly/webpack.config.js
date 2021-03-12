const path = require("path");
const EsmWebpackPlugin = require("@purtuga/esm-webpack-plugin");

module.exports = {
    entry: "./wwwroot/js/esm-bundle/index.js",
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            }
        ]
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/js'),
        filename: "esm-bundle.js",
        library: "Teronis_Microsoft_JSInterop_Test_WebAssembly",
        libraryTarget: "var"
    },
    plugins: [
        new EsmWebpackPlugin()
    ]
};
