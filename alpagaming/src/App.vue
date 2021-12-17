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
    apis: [
      { 
        name: 'beluga',
        url: '',
        token: ""
      },
      { 
        name: 'thikler',
        url: '',
        token: ""
      },
      { 
        name: 'bigot',
        url: '',
        token: ""
      },
      { 
        name: 'william',
        url: '',
        token: ""
      }          
    ],
    dataAuth : {}
  }),

  async mounted(){
    await this.initApisUrl()
    await this.getListGames()
  },

  methods:{
      async getListGames(){
        await this.auth();
        await this.initApisUrl()
        this.apis.forEach(async api => {
          switch(api.name){
            case "beluga":
              api.url +="games";
              break;
            case "bigot":
              api.url +="games";
              break;
            case "thikler":
              api.url += "List/VideoGames/Name"
              break;
            case "william":
              api.url += "games"
              break;
            default:
              break;
          }

          let rawResult = await axios({
                  method: 'get',
                  url: api.url,
                  headers: {
                    Authorization: api.token
                  }, 
                });
          if(rawResult.data.$values != null){
            for(let game of  rawResult.data.$values){
              this.games.push(game)
            }    
          }
          else if(rawResult.data.result != null){
            for(let game of  rawResult.data.result){
              this.games.push(game)
            }    
          }
          else{
            for(let game of  rawResult.data){
              this.games.push(game)
            }    
          }           
        });
    },
      async auth(){
        for (let api of this.apis) {
          switch(api.name){
            case 'beluga':
              api.url+= "auth"
              api.token = "Bearer "
              this.dataAuth = {
                username: "admin",
                password: "patafoin"
              }
              break;
            case 'bigot':
              api.url+= "users/login"
              this.dataAuth = {
                email: "quentin@live.fr",
                password: "123456aA"
              }
              break; 
            case 'thikler' :
              api.token = "Bearer "
              api.url+= "auth"
              this.dataAuth = {
                username: "Klervia",
                password: "Thibaut"
              }              
              break;
            case 'william' :
              api.url+= "signIn"
              this.dataAuth = {
                email: "cocodu53",
                password: "erwan"
              }              
              break;
            default:
              break;       
          }


          let rawResult = await axios({
              method: 'post',
              url: api.url,
              headers: {}, 
              data: this.dataAuth
          });  
          if(rawResult.data.token != null){
            api.token += rawResult.data.token 
          }
          else{
            api.token = rawResult.data.accessToken 
          }
        }      
    },

    async initApisUrl(){
      this.apis.forEach(api => {
        switch(api.name){
          case "beluga":
            api.url = "https://belugaming.herokuapp.com/api/"
            break;
          case "bigot":
            api.url = "https://bigogaming.herokuapp.com/"; 
            break;
          case "thikler":
            api.url = "https://thikler.herokuapp.com/api/"
            break;
          case "william":
            api.url = "https://bibliojvapi.herokuapp.com/api/"
            break;
          default:
            break;
        }
    });
    }
  }
};
</script>

