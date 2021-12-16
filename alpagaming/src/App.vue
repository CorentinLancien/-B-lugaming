<template>
  <v-app>
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
    }
  }

};
</script>
