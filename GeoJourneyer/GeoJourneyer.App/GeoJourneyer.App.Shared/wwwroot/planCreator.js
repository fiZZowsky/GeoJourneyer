window.planCreator = {
    map: null,
    searchService: null,
    markers: { search: [], selected: [] },
    listEl: null,
    dotnet: null,
    init: function (mapId, listId, dotnet) {
        this.dotnet = dotnet;
        this.listEl = document.getElementById(listId);
        this.map = new google.maps.Map(document.getElementById(mapId), {
            zoom: 5,
            center: { lat: 0, lng: 0 }
        });
        this.searchService = new google.maps.places.PlacesService(this.map);
        Sortable.create(this.listEl, {
            onEnd: () => {
                const ids = Array.from(this.listEl.children).map(li => parseInt(li.getAttribute('data-id')));
                if (this.dotnet) this.dotnet.invokeMethodAsync('UpdateOrder', ids);
            }
        });
    },
    clearMarkers(type) {
        this.markers[type].forEach(m => m.setMap(null));
        this.markers[type] = [];
    },
    search: function (query) {
        this.clearMarkers('search');
        if (!query) {
            if (this.dotnet) this.dotnet.invokeMethodAsync('ReceiveSearchResults', []);
            return;
        }
        this.searchService.textSearch({ query: query }, (results, status) => {
            if (status !== 'OK') return;
            const places = results.map(r => ({
                id: r.place_id.hashCode(),
                name: r.name,
                lat: r.geometry.location.lat(),
                lng: r.geometry.location.lng()
            }));
            if (this.dotnet) this.dotnet.invokeMethodAsync('ReceiveSearchResults', places);
            results.forEach(r => {
                const m = new google.maps.Marker({
                    position: r.geometry.location,
                    map: this.map,
                    icon: 'http://maps.google.com/mapfiles/ms/icons/blue-dot.png'
                });
                this.markers.search.push(m);
            });
            if (results[0]) this.map.setCenter(results[0].geometry.location);
        });
    },
    addSelectedMarker: function (id) {
        const li = Array.from(this.listEl.children).find(el => parseInt(el.getAttribute('data-id')) === id);
        if (!li) return;
        const name = li.textContent;
        const lat = parseFloat(li.getAttribute('data-lat'));
        const lng = parseFloat(li.getAttribute('data-lng'));
        const marker = new google.maps.Marker({
            position: { lat: lat, lng: lng },
            map: this.map,
            icon: 'http://maps.google.com/mapfiles/ms/icons/red-dot.png'
        });
        this.markers.selected.push(marker);
    }
};
String.prototype.hashCode = function () { var h = 0; for (var i = 0; i < this.length; i++) { h = (Math.imul(31, h) + this.charCodeAt(i)) | 0; } return h; };