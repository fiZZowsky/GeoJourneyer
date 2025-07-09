window.initGlobe = function (visited, dotNetHelper) {
    const container = document.getElementById('globeContainer');
    if (!container) return;
    if (typeof Globe === 'undefined' || typeof topojson === 'undefined') {
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('GlobeLoaded');
        }
        return;
    }

    if (!window.globeInstance) {
        window.globeInstance = Globe()(container)
            .globeImageUrl(null)
            .bumpImageUrl(null);
        fetch('_content/GeoJourneyer.App.Shared/countries-50m.json')
            .then(res => res.json())
            .then(worldData => {
                const countries = window.topojson.feature(worldData, worldData.objects.countries).features;
                window.countriesData = countries;
                window.globeInstance
                    .polygonsData(countries)
                    .polygonAltitude(0.06)
                    .polygonSideColor(() => 'rgba(0,0,0,0)')
                    .polygonStrokeColor(() => '#000')
                    .onPolygonClick(d => {
                        const name = d.properties.name;
                        if (!Array.isArray(window.visitedCountries)) {
                            window.visitedCountries = [];
                        }
                        if (window.visitedCountries.indexOf(name) >= 0) {
                            window.visitedCountries = window.visitedCountries.filter(c => c !== name);
                        } else {
                            window.visitedCountries.push(name);
                        }
                        window.updateVisitedCountries(window.visitedCountries);
                        if (window.dotNetHelper) {
                            window.dotNetHelper.invokeMethodAsync('ToggleCountry', name);
                        }
                    });

                window.updateVisitedCountries(visited || []);
                // TODO: not wordking
                //if (dotNetHelper) {
                //    dotNetHelper.invokeMethodAsync('GlobeLoaded');
                //}
            })
    } else {
        window.updateVisitedCountries(visited || []);
    }
    window.dotNetHelper = dotNetHelper;
}

window.updateVisitedCountries = function (visited) {
    window.visitedCountries = Array.isArray(visited) ? visited : [];
    if (!window.globeInstance) return;

    const capColor = d => window.visitedCountries.indexOf(d.properties.name) >= 0
        ? 'rgba(100, 255, 100, 0.8)'
        : '#ffffff';
    window.globeInstance.polygonCapColor(capColor);

    if (window.countriesData) {
        window.globeInstance.polygonsData(window.countriesData);
    }
}