<template>
  <v-app>
    <v-card>
    <v-container fluid>
            <v-row
        align="center"
      >
        <v-col cols="5">
          <v-autocomplete
            v-model="games"
            :items="games"
            dense
            chips
            small-chips
            label="Name"
            solo
          ></v-autocomplete>
        </v-col>
      </v-row>
      <v-row
        align="center"
      >
        <v-col cols="12">
          <v-autocomplete
            v-model="games"
            :items="games"
            dense
            chips
            small-chips
            label="Categories"
            multiple
            solo
          ></v-autocomplete>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
    <v-card>
      <v-card-title>
        Jeux Vidéos
        <v-spacer></v-spacer>
        <v-text-field
          v-model="search"
          append-icon="mdi-magnify"
          label="Rechercher"
          single-line
          hide-details
        ></v-text-field>
      </v-card-title>
      <v-data-table
        :headers="headers"
        :items="games"
        :search="search"
        loading=isLoading
        loading-text="Loading... Please wait"
      ></v-data-table>
    </v-card>
  </v-app>
</template>

<script>
const axios = require('axios');
export default {
  name: 'App',
  data: () => ({
    search: '',
    headers: [
      {
        text: 'Nom',
        align: 'start',
        sortable: true,
        value: 'name',
      },
      { text: 'Date', value: 'date' },
      { text: 'Prix TTC', value: 'prix' },
    ],
    games : [],
    token : "",
  }),

  async mounted(){
    await this.auth()
    await this.getListGames()
  },

  methods:{
      async getListGames(){
        let url = 'https://belugaming.herokuapp.com/api/games';
        let rawResult = await axios({
                method: 'get',
                url: url,
                headers: {
                  Authorization: `Bearer ` + this.token
                }, 
              });
       this.games = rawResult.data
    },
    async auth(){
      let url = 'https://belugaming.herokuapp.com/api/auth';
      let rawResult = await axios({
              method: 'post',
              url: url,
              headers: {}, 
              data: {
                username: 'admin',
                password: 'patafoin'
              }
            });
      
      this.token = rawResult.data.token
    },
  }

};
</script>
