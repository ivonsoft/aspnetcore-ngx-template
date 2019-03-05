var urlToImage = require('url-to-image');
module.exports = function (callback, url, imageName) {
    urlToImage(url, imageName).then(function () {
        callback(null, imageName);
    }).catch(function (err) {
        callback(err, imageName);
    });
};