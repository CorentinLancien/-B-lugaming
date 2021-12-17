<template>
  <v-app>
    <v-card>
      <v-container fluid>
        <v-row>
          <v-col cols="10">
            <v-row align="center">
              <v-col cols="5">
                <v-autocomplete
                  v-model="selectedName"
                  :items="games"
                  item-value="name"
                  item-text="name"
                  dense
                  chips
                  small-chips
                  label="Name"
                  solo
                ></v-autocomplete>
              </v-col>
              <v-col cols="5">
                <v-text-field
                  v-model="selectedYear"
                  dense
                  chips
                  small-chips
                  solo
                  min="2010"
                  max="2022"
                  type="number"
                  label="Année"
                />
              </v-col>
            </v-row>
            <v-row align="center">
              <v-col cols="5">
                <v-autocomplete
                  v-model="selectedCategories"
                  :items="games"
                  item-value="categories.Name"
                  item-text="categories.Name"
                  dense
                  chips
                  small-chips
                  label="Categories"
                  multiple
                  solo
                ></v-autocomplete>
              </v-col>
              <v-col cols="5">
                <v-text-field
                  v-model="selectedPrix"
                  dense
                  chips
                  small-chips
                  solo
                  min="0"
                  max="150"
                  type="number"
                  label="Prix"
                />
              </v-col>
            </v-row>
          </v-col>
          <v-col cols="2" align="center">
            <v-btn
              color="primary"
              elevation="3"
              outlined
              @click="this.showSelected()"
            >Rechercher</v-btn>
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
      >
        <template v-slot:item.categories="{ items }">
            <v-chip
              v-for="item in items"
              :key="item.Id"
              :color="blue"
              dark
            >
              {{  }}
            </v-chip>
        </template>
      </v-data-table>
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
      { text: 'Categories', value: 'categories' },
    ],
    games : [],
    token : "",
    selectedName : "",
    selectedYear : "[]",
    selectedCategories : [],
    selectedPrix : "",
  }),

  async mounted(){
    await this.auth()
    await this.getListGames()
    console.log(this.games)
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
    showSelected(){
      console.log(this.selectedName + "  " + this.selectedYear + "  " + this.selectedCategories + "  " + this.selectedPrix);
    }
  }

};
</script>
