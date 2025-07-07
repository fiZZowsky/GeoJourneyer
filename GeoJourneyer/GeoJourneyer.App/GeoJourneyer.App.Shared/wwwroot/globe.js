window.initGlobe = function (visited) {
    const container = document.getElementById('globeContainer');
    if (!container) return;

    if (!window.globeInstance) {
        window.globeInstance = Globe()(container);
        fetch('https://unpkg.com/world-atlas@2.0.2/countries-110m.json')
            .then(res => res.json())
            .then(worldData => {
                const countries = window.topojson.feature(worldData, worldData.objects.countries).features;
                window.countriesData = countries;
                window.globeInstance
                    .polygonsData(countries)
                    .polygonAltitude(0.06)
                    .polygonSideColor(() => 'rgba(0,0,0,0)')
                    .polygonStrokeColor(() => '#000');
                window.updateVisitedCountries(visited || []);
            });
    } else {
        window.updateVisitedCountries(visited || []);
    }
}

window.updateVisitedCountries = function (visited) {
    window.visitedCountries = visited || [];
    if (!window.globeInstance) return;

    const capColor = d => window.visitedCountries.indexOf(d.properties.name) >= 0
        ? 'rgba(100, 255, 100, 0.8)'
        : '#ffffff';
    window.globeInstance.polygonCapColor(capColor);

    if (window.countriesData) {
        window.globeInstance.polygonsData(window.countriesData);
    }
}