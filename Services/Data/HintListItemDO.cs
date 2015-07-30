namespace FindMyFamilies.Data {

    public class HintListItemDO : FindListItemDO {
        public HintListItemDO() {
        } 

        public string Hints { get; set; }
        public int Count { get; set; }
        public double TopScore { get; set; }
	}
}
