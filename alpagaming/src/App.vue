<template>
  <v-app>
     {{ token }}
    <v-card
      class="mx-auto"
      max-width="300"
      tile
    >
      <v-list dense>
        <v-subheader>REPORTS</v-subheader>
        <v-list-item-group
          v-model="games"
          color="primary"
        >
          <v-list-item
            v-for="game in games"
            :key="game.Id"
          >
            <v-list-item-content>
              <v-list-item-title v-text="game.name"></v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-card>
  </v-app>
</template>

<script>
const axios = require('axios');
export default {
  name: 'App',


  data: () => ({
    games : [],
    token : "",
  }),

  async mounted(){
    await this.auth()
    await this.getListGames()
    console.log(this.games)
  },

  methods:{
      async getListGames(){
        const https = require('https');
        const httpsAgent = new https.Agent({ rejectUnauthorized: false });

        let url = 'https://localhost:7069/api/games';
        let rawResult = await axios.get(url, {
                httpsAgent
            });

        this.games = rawResult.data.$values
    },
        async auth(){
        const https = require('https');
        const httpsAgent = new https.Agent({ rejectUnauthorized: false });

        let url = 'https://localhost:7069/api/auth';
        let rawResult = await axios.post(url, {
                httpsAgent,
                username: 'admin',
                lastName: 'patafoin'
            });

        this.token = rawResult.data.$values
    }
  }

};
</script>
