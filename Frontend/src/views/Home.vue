<template>
  <div class="home">
    <h1>Climate Data</h1>
    <b-table
      :data="climateData"
      :paginated="isPaginated"
      :per-page="perPage"
      :current-page.sync="currentPage"
      :pagination-simple="isPaginationSimple"
      :columns="columns"
    ></b-table>
    <div id="myDiv"></div>
  </div>
</template>

<script>
// @ is an alias to /src
import axios from "axios";

export default {
  name: "home",
  data() {
    return {
      climateData: [],
      airQuality: [],
      isPaginated: true,
      currentPage: 1,
      perPage: 10,
      isPaginationSimple: true,
      columns: [
        {
          field: "temperature",
          label: "Temperature",
          width: "40"
        },
        {
          field: "humadity",
          label: "Humidity"
        },
        {
          field: "time",
          label: "Time"
        }
      ]
    };
  },
  components: {},
  mounted() {
    var myData = this;
    axios
      .get("http://hackhire.azurewebsites.net/api/iot/dht")
      .then(function(response) {
        if (response.data) myData.climateData = response.data;
        var temp = [],
          humi = [],
          date = [];
        for (var data of myData.climateData) {
          temp.push(data.temperature);
          humi.push(data.humadity);
          date.push(data.time);
        }
        var trace1 = {
          type: "scatter",
          mode: "lines",
          name: "Temperature",
          x: date,
          y: temp,
          line: { color: "#17BECF" }
        };

        var trace2 = {
          type: "scatter",
          mode: "lines",
          name: "Humidity",
          x: date,
          y: humi,
          line: { color: "#7F7F7F" }
        };

        var data = [trace1, trace2];
        var layout = {
          title: "Temperature",
          xaxis: {
            range: ["2019-01-26 13:02", "2019-01-26 13:06"],
            type: "date"
          }
        };
        window.Plotly.newPlot("myDiv", data, layout);
      });
  }
};
</script>
