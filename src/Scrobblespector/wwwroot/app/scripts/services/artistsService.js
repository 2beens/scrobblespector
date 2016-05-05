(function () {
    angular
        .module('scrobblespectorApp')
        .factory('artistsService', artistsService);
    
    artistsService.$inject = ['$resource', 'utilizr'];

    function artistsService($resource, utilizr) {
        var artistsApiPath = utilizr.getArtistsApiPath() + '/:artistName';
        var searchArtistsApiPath = utilizr.getArtistsApiPath() + '/search/:artistName';
        var Artists = $resource(artistsApiPath, { artistName: '@artistName' });
        var ArtistsSearch = $resource(searchArtistsApiPath, { artistName: '@artistName' });

        var service = {
            searchArtist: searchArtist,
            getArtist: getArtist
        };

        return service;

        ///////////////////////////////////////////////

        function searchArtist(artistName) {
            return ArtistsSearch.get({ artistName: artistName }).$promise;
        }

        function getArtist(artistName) {
            return Artists.get({ artistName: artistName }).$promise;
        }
    }
})();