<template>
  <div>
    <page-title title="Sonar" />
    <div ref="chartContainer" style="height:600px;">
      <canvas ref="chart" id="canvas"></canvas>
    </div>
    <button @click="snapshot">Export me</button>
  </div>
</template>
<script>
// Import ChartJS
import Chart from 'chart.js'

// Colors
var chartColors = [ 'rgb(255, 99, 132)', 'rgb(255, 159, 64)', 'rgb(255, 205, 86)', 'rgb(75, 192, 192)', 'rgb(54, 162, 235)', 'rgb(153, 102, 255)', 'rgb(201, 203, 207)' ]

Chart.pluginService.register({
  beforeDraw: function (chart, easing) {
    if (chart.config.options.chartArea && chart.config.options.chartArea.backgroundColor) {
      var ctx = chart.chart.ctx;

      ctx.save();
      ctx.fillStyle = chart.config.options.chartArea.backgroundColor;
      // Paint only the back of the chart
      ////var chartArea = chart.chartArea;
      ////ctx.fillRect(chartArea.left, chartArea.top, chartArea.right - chartArea.left, chartArea.bottom - chartArea.top);

      // Paint the full chart background.
      ctx.fillRect(0, 0, ctx.canvas.width, ctx.canvas.height);

      ctx.restore();
    }
  }
});

export default {
  data () {
    return {
      chart: null,
      chartConfig: {
        type: 'line',
        data: {
          labels: [],
          datasets: []
        },
        options: {
          chartArea: {
            backgroundColor: "white"
          },
          responsive: true,
          maintainAspectRatio: false,
          title: {
            display: true,
            text: 'Your Chart.Js To Push'
          },
          tooltips: {
            mode: 'index',
            intersect: false,
          },
          scales: {
            xAxes: [{
              display: true,
              type: 'time',
              scaleLabel: {
                display: true,
                labelString: 'Date'
              }
            }],
            yAxes: [{
              display: true,
              scaleLabel: {
                display: true,
                labelString: 'Value'
              }
            }]
          }
        }
      }
    }
  },
  mounted () {
    this.chart = new Chart(this.$refs.chart, this.chartConfig)
    this._loadData()
  },
  methods: {
    // Fake the data
    _loadData: async function () {
      data = {
        measures: [
          {metric: 'Curve A', history: []},
          {metric: 'Curve B', history: []},
        ]
      }

      // Generate some fake data
      await data.measures.forEach(measure => {
        for (var i=1; i<= 12; i++) {
          measure.history.push({date: `2018-${ i.toString().padStart(2, '0')}-01`, value: Math.random()% 20 * 20 })
        }
      });

      // We now have measures + data
      var measures = data.measures
      let allLabels = []
      for (var m = 0; m < measures.length; m++) {
        let currentColor = chartColors[m % chartColors.length]
        var data = {
          label: measures[m].metric,
          backgroundColor: currentColor,
          borderColor: currentColor,
          tension: 0.5, // If you don't want bezier curves
          data: [],
          fill: false,
        }

        for (var h = 0; h < measures[m].history.length; h++) {
          let date = new Date(measures[m].history[h].date)
          if (allLabels.indexOf(date) < 0) {
            allLabels.push(date)
          }

          data.data.push(measures[m].history[h].value)
        }

        this.chartConfig.data.datasets.push(data)
      }

      this.chartConfig.data.labels = allLabels
      this.chart.update()
    },

    snapshot: function () {
      this.$refs.chartContainer.style.width = '1200px'
      this.chart.resize()
      var ctx = this

      // Do the Snapshot
      this.$refs.chart.toBlob(function (blob) {
        // Resize to original size
        ctx.$refs.chartContainer.style.width = ""
        ctx.chart.resize()

        // Prepare the form post
        var data = new FormData()
        data.append('file', blob, `${ctx.chartConfig.options.title.text}.png`)
        const config = {
            headers: { 'content-type': 'multipart/form-data' }
        }

        // Post the file
        ctx.$http.post('/api/images/confluence/67657373', data, config)
      }, "image/png", 0.95);
    }
  }
}
</script>
