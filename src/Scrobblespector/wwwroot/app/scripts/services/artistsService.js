﻿(function () {
    angular
        .module('scrobblespectorApp')
        .factory('artistsService', artistsService);
    
    artistsService.$inject = ['$resource', 'utilizr'];

    function artistsService($resource, utilizr) {
        var artistsApiPath = utilizr.getArtistsApiPath() + '/:mbid';
        var searchArtistsApiPath = utilizr.getArtistsApiPath() + '/search/:artistName';
        var similarArtistsApiPath = utilizr.getArtistsApiPath() + '/similar/:mbid';

        var Artists = $resource(artistsApiPath, { mbid: '@mbid' });
        var ArtistsSearch = $resource(searchArtistsApiPath, { artistName: '@artistName' });
        var ArtistsSimilar = $resource(similarArtistsApiPath, { mbid: '@mbid' });

        var service = {
            searchArtist: searchArtist,
            getArtist: getArtist,
            getSimilarArtists: getSimilarArtists
        };

        return service;

        ///////////////////////////////////////////////

        function searchArtist(artistName) {
            return ArtistsSearch.get({ artistName: artistName }).$promise;
        }

        function getArtist(mbid) {
            return Artists.get({ mbid: mbid }).$promise;
        }

        function getSimilarArtists(mbid) {
            return ArtistsSimilar.get({ mbid: mbid }).$promise;
        }
    }
})();