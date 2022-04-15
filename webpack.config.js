module.exports = {
    resolve: {
        fallback: {
            crypto: require.resolve('crypto-browserify'),
            util: require.resolve('util/'),
            stream: require.resolve('stream-browserify')
        },
    },
};
