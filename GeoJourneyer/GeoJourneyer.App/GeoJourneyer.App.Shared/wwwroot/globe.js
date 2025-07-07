window.initGlobe = function (visited, dotNetHelper) {
    const container = document.getElementById('globeContainer');
    if (!container) return;

    if (!window.globeInstance) {
        window.globeInstance = Globe()(container)
            .globeImageUrl(null)
            .bumpImageUrl(null);
        fetch('https://unpkg.com/world-atlas@2.0.2/countries-110m.json')
            .then(res => res.json())
            .then(worldData => {
                const countries = window.topojson.feature(worldData, worldData.objects.countries).features;
                window.countriesData = countries;
                const labels = countries.map(c => {
                    const center = (function (feature) {
                        const coords = [];
                        if (feature.geometry.type === 'Polygon') {
                            feature.geometry.coordinates.forEach(p => coords.push(...p));
                        } else if (feature.geometry.type === 'MultiPolygon') {
                            feature.geometry.coordinates.forEach(mp => mp.forEach(p => coords.push(...p)));
                        }
                        const total = coords.length;
                        const sum = coords.reduce((acc, cur) => [acc[0] + cur[0], acc[1] + cur[1]], [0, 0]);
                        return [sum[0] / total, sum[1] / total];
                    })(c);
                    return { lat: center[1], lon: center[0], name: c.properties.name };
                });
                window.globeInstance
                    .polygonsData(countries)
                    .polygonAltitude(0.06)
                    .polygonSideColor(() => 'rgba(0,0,0,0)')
                    .polygonStrokeColor(() => '#000')
                    .onPolygonClick(d => {
                        const name = d.properties.name;
                        if (!window.visitedCountries) window.visitedCountries = [];
                        if (window.visitedCountries.indexOf(name) >= 0) {
                            window.visitedCountries = window.visitedCountries.filter(c => c !== name);
                        } else {
                            window.visitedCountries.push(name);
                        }
                        window.updateVisitedCountries(window.visitedCountries);
                        if (window.dotNetHelper) {
                            window.dotNetHelper.invokeMethodAsync('ToggleCountry', name);
                        }
                    })
                    .labelsData(labels)
                    .labelLat(d => d.lat)
                    .labelLng(d => d.lon)
                    .labelText(d => d.name)
                    .labelColor(() => 'black')
                    .labelSize(0.6)
                    .labelDotRadius(0)
                    .labelAltitude(0.07);
                window.updateVisitedCountries(visited || []);
            });
    } else {
        window.updateVisitedCountries(visited || []);
    }
    window.dotNetHelper = dotNetHelper;
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