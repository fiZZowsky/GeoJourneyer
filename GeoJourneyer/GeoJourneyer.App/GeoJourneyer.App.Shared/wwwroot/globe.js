window.initGlobe = function () {
    if (window.globeInstance) return;
    const container = document.getElementById('globeContainer');
    if (!container) return;
    window.globeInstance = Globe()(container);

    fetch('https://unpkg.com/world-atlas@2.0.2/countries-110m.json').then(res => res.json()).then(worldData => {
        const countries = window.topojson.feature(worldData, worldData.objects.countries).features;
        window.globeInstance
            .polygonsData(countries)
            .polygonAltitude(0.06)
            .polygonCapColor(() => 'rgba(100, 200, 100, 0.7)')
            .polygonSideColor(() => 'rgba(50, 150, 50, 0.4)')
            .polygonStrokeColor(() => '#111');
    });
}