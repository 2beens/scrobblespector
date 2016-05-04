(function () {
    angular
        .module('scrobblespectorApp')
        .factory('artistsService', artistsService);
    
    artistsService.$inject = ['$resource', 'utilizr'];

    function artistsService($resource, utilizr) {
        var artistsApiPath = utilizr.getArtistsApiPath() + '/:artistName';
        var Artists = $resource(artistsApiPath, { artistName: '@artistName' });

        var service = {
            searchArtist: searchArtist
        };

        return service;

        ///////////////////////////////////////////////

        function searchArtist(artistName) {
            return Artists.get({ artistName: artistName }).$promise;
        }
    }
})();